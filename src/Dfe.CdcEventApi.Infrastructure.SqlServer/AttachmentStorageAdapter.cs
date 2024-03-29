﻿namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web;
    using Azure;
    using Azure.Storage;
    using Azure.Storage.Files.Shares;
    using Azure.Storage.Sas;
    using Dapper;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Implements <see cref="IAttachmentStorageAdapter" />.
    /// This adapter provids two services;
    /// Get a list of required attachment file details <see cref="AttachmentRequest"/>.
    /// Recieve the obtained attachment data <see cref="AttachmentResponse"/>.
    /// </summary>
    public class AttachmentStorageAdapter : IAttachmentStorageAdapter
    {
        private const string EXTRACTAttachmentMerge = "EXTRACT-Attachment-Merge";
        private const string EXTRACTAttachmentFileInfo = "EXTRACT-Attachment-File-Info";
        private const string EXTRACTAttachmentList = "EXTRACT-Attachment-List";
        private const string EXTRACTAttachmentToDeleteList = "EXTRACT-Attachment-To-Delete-List";
        private const string EXTRACTAttachmentAddDeletedEvidenceItem = "EXTRACT-Attachment-Add-Deleted_EvidenceItem";
        private const string MASTERFileData = "MASTER-File-Data";
        private const string MASTERFileDataDelete = "MASTER-File-Data-Delete";
        private const int CommandTimeoutAsLongAsItTakes = 0;
        private const string ProcessHandlerFileNameFormat = "{0}.sql";
        private readonly string rawDbConnectionString;
        private readonly string masteredDbConnectionString;
        private readonly string attachmentStorageConnectionString;
        private readonly string attachmentStorageAccountName;
        private readonly string attachmentStorageAccountKey;
        private readonly Assembly assembly;
        private readonly ILoggerProvider loggerProvider;
        private readonly IBlobConvertor blobConvertor;
        private readonly string controlHandlersPath;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="AttachmentStorageAdapter"/> class.
        /// </summary>
        /// <param name="entityStorageAdapterSettingsProvider">
        /// An instance of type
        /// <see cref="IEntityStorageAdapterSettingsProvider" />.
        /// </param>
        /// <param name="blobConvertor">An instance of <see cref="IBlobConvertor"/>.</param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        /// <param name="attachmentSettingsProvider">An instance of <see cref="IAttachmentSettingsProvider"/>.</param>
        public AttachmentStorageAdapter(
            IEntityStorageAdapterSettingsProvider entityStorageAdapterSettingsProvider,
            IBlobConvertor blobConvertor,
            ILoggerProvider loggerProvider,
            IAttachmentSettingsProvider attachmentSettingsProvider)
        {
            if (entityStorageAdapterSettingsProvider == null)
            {
                throw new ArgumentNullException(
                    nameof(entityStorageAdapterSettingsProvider));
            }

            this.blobConvertor = blobConvertor ?? throw new ArgumentNullException(
                   nameof(blobConvertor));

            this.loggerProvider = loggerProvider ?? throw new ArgumentNullException(
                   nameof(loggerProvider));

            if (attachmentSettingsProvider == null)
            {
                throw new ArgumentNullException(nameof(attachmentSettingsProvider));
            }

            Type type = typeof(ControlStorageAdapter);

            this.assembly = type.Assembly;

            this.controlHandlersPath = $"{type.Namespace}.AttachmentHandlers";

            this.rawDbConnectionString = entityStorageAdapterSettingsProvider.RawDbConnectionString;
            this.masteredDbConnectionString = entityStorageAdapterSettingsProvider.MasteredDbConnectionString;

            this.attachmentStorageConnectionString = attachmentSettingsProvider.AttachmentStorageConnectionString;
            this.attachmentStorageAccountName = attachmentSettingsProvider.AttachmentStorageAccountName;
            this.attachmentStorageAccountKey = attachmentSettingsProvider.AttachmentStorageAccountKey;
        }

        /// <summary>
        /// Creates many <see cref="AttachmentResponse"/> records.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <param name="attachments">
        /// A collection of <see cref="AttachmentResponse"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/>.</returns>
        public async Task CreateAsync(
            DateTime runIdentifier,
            IEnumerable<AttachmentResponse> attachments)
        {
            if (attachments == null)
            {
                throw new ArgumentNullException(nameof(attachments));
            }

            this.loggerProvider.Info($"Starting a blob load at {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            SqlTransaction transaction = null;
            SqlConnection sqlConnection = null;
            SqlConnection masteredSqlConnection = null;
            try
            {
                sqlConnection = new SqlConnection(this.rawDbConnectionString);
#pragma warning disable CA2000 // Dispose objects before losing scope
                masteredSqlConnection = new SqlConnection(this.masteredDbConnectionString);
#pragma warning restore CA2000 // Dispose objects before losing scope

                sqlConnection.Open();
                masteredSqlConnection.Open();
                this.loggerProvider.Info($"Opened 2 SQL Connections");

                transaction = sqlConnection.BeginTransaction();

                this.loggerProvider.Info($"Started transaction on primary connection");

                var insertSql = this.ExtractHandler(EXTRACTAttachmentMerge);
                var upsertSql = this.ExtractHandler(EXTRACTAttachmentFileInfo);

                this.loggerProvider.Debug($"Creating Storage Blob records.");

                stopwatch.Start();

                // despite capability of handling a collection of objects there is usually only one.
                foreach (var blob in attachments)
                {
                    this.loggerProvider.Info($"Storing obtained attachment data.");
                    var item = await this.StoreAttachment(blob).ConfigureAwait(false);
                    this.loggerProvider.Info($"Storing obtained attachment data.");

                    // update the ETL model evidence metadata records.
                    this.loggerProvider.Info($"Storing obtained attachment metadata and file info references");

                    // add it as a known blob
                    await sqlConnection
                        .ExecuteAsync(insertSql, item, transaction, CommandTimeoutAsLongAsItTakes)
                        .ConfigureAwait(false);
                    this.loggerProvider.Info($"Stored obtained attachment metadata");

                    await masteredSqlConnection
                        .ExecuteAsync(upsertSql, item, null, CommandTimeoutAsLongAsItTakes)
                        .ConfigureAwait(false);
                    this.loggerProvider.Info($"Stored obtained attachment file info references");
                }

                transaction.Commit();
                stopwatch.Stop();
                this.loggerProvider.Info($"Commited Transaction.");

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                this.loggerProvider.Error($"Exception. {ex}");
                throw;
            }
            finally
            {
                if (sqlConnection?.State != System.Data.ConnectionState.Closed)
                {
                    this.loggerProvider.Info($"Closing primary database connection.");
                    sqlConnection?.Close();
                    sqlConnection?.Dispose();
                }

                sqlConnection?.Dispose();

                if (masteredSqlConnection?.State != System.Data.ConnectionState.Closed)
                {
                    this.loggerProvider.Info($"Closing secondary database connection.");
                    masteredSqlConnection?.Close();
                    masteredSqlConnection?.Dispose();
                }
            }
        }

        /// <inheritdoc/>
        public Task<IEnumerable<AttachmentRequest>> GetAsync()
        {
            this.loggerProvider.Info($"Getting required attachments list.");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                string querySql = this.ExtractHandler(EXTRACTAttachmentList);
                this.loggerProvider.Debug($"Retrieving attachment records.");

                stopwatch.Start();

                var attachments = sqlConnection.Query<AttachmentRequest>(
                                            querySql,
                                            null,
                                            null,
                                            true,
                                            CommandTimeoutAsLongAsItTakes);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return Task.FromResult(attachments);
            }
        }

        /// <inheritdoc/>
        public Task<IEnumerable<AttachmentForDeletionRequest>> GetForDeletionAsync()
        {
            this.loggerProvider.Info($"Getting list of attachments for deletion.");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                string querySql = this.ExtractHandler(EXTRACTAttachmentToDeleteList);
                this.loggerProvider.Debug($"Retrieving attachment for deletion records.");

                stopwatch.Start();

                var attachments = sqlConnection.Query<AttachmentForDeletionRequest>(
                                            querySql,
                                            null,
                                            null,
                                            true,
                                            CommandTimeoutAsLongAsItTakes);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return Task.FromResult(attachments);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AttachmentForDeletionRequest>> DeleteAsync()
        {
            var attachments = await this.GetForDeletionAsync().ConfigureAwait(false);

            foreach (var attachment in attachments)
            {
                Stopwatch stopwatch = new Stopwatch();
                SqlTransaction transaction = null;
                SqlConnection sqlConnection = null;
                SqlConnection masteredSqlConnection = null;
                try
                {
                    var masteredSelectSql = this.ExtractHandler(MASTERFileData);
                    var masteredDeleteSql = this.ExtractHandler(MASTERFileDataDelete);
                    var rawInsertSql = this.ExtractHandler(EXTRACTAttachmentAddDeletedEvidenceItem);

                    sqlConnection = new SqlConnection(this.rawDbConnectionString);
#pragma warning disable CA2000 // Dispose objects before losing scope
                    masteredSqlConnection = new SqlConnection(this.masteredDbConnectionString);
#pragma warning restore CA2000 // Dispose objects before losing scope

                    sqlConnection.Open();
                    masteredSqlConnection.Open();
                    this.loggerProvider.Info($"Opened 2 SQL Connections");

                    this.loggerProvider.Debug($"Retrieving file data for blob key {attachment.BlobKey}.");

                    stopwatch.Start();

                    var fileData = masteredSqlConnection.Query<FileData>(
                                                masteredSelectSql,
                                                new { SupplierKeyID = attachment.BlobKey },
                                                null,
                                                true,
                                                CommandTimeoutAsLongAsItTakes);

                    stopwatch.Stop();

                    TimeSpan elapsed = stopwatch.Elapsed;

                    if (fileData.Any())
                    {
                        var fileUrl = fileData.First().FileURL;

                        this.loggerProvider.Info(
                            $"File data for {fileUrl} retrieved, time elapsed: " +
                            $"{elapsed}.");

                        transaction = sqlConnection.BeginTransaction();

                        this.loggerProvider.Info($"Started transaction on primary connection");

                        this.loggerProvider.Info($"Attempting deletion of blob from file share");

                        StorageSharedKeyCredential credential = new StorageSharedKeyCredential(this.attachmentStorageAccountName, this.attachmentStorageAccountKey);

                        ShareFileClient shareFileClient = new ShareFileClient(new Uri(fileUrl), credential);

                        await shareFileClient.DeleteAsync().ConfigureAwait(false);

                        this.loggerProvider.Debug($"Deleting Storage Blob record.");

                        stopwatch.Start();

                        await masteredSqlConnection
                            .ExecuteAsync(masteredDeleteSql, new { SupplierKeyID = attachment.BlobKey }, null, CommandTimeoutAsLongAsItTakes)
                            .ConfigureAwait(false);
                        this.loggerProvider.Info($"Deleted file data from mastered db");

                        await sqlConnection
                            .ExecuteAsync(rawInsertSql, attachment, transaction, CommandTimeoutAsLongAsItTakes)
                            .ConfigureAwait(false);
                        this.loggerProvider.Info($"Added Delete_EvidenceItem record to raw db");

                        transaction.Commit();
                        stopwatch.Stop();
                        this.loggerProvider.Info($"Commited Transaction.");

                        elapsed = stopwatch.Elapsed;

                        this.loggerProvider.Info(
                            $"Query executed with success, time elapsed: " +
                            $"{elapsed}.");
                    }
                    else
                    {
                        this.loggerProvider.Warning($"File data not found for {attachment.BlobKey}");
                    }
                }
                catch (Exception ex)
                {
                    transaction?.Rollback();
                    this.loggerProvider.Error($"Exception. {ex}");
                    throw;
                }
                finally
                {
                    if (sqlConnection?.State != System.Data.ConnectionState.Closed)
                    {
                        this.loggerProvider.Info($"Closing primary database connection.");
                        sqlConnection?.Close();
                        sqlConnection?.Dispose();
                    }

                    sqlConnection?.Dispose();

                    if (masteredSqlConnection?.State != System.Data.ConnectionState.Closed)
                    {
                        this.loggerProvider.Info($"Closing secondary database connection.");
                        masteredSqlConnection?.Close();
                        masteredSqlConnection?.Dispose();
                    }
                }
            }

            return attachments;
        }

        private async Task<AttachmentResponse> StoreAttachment(AttachmentResponse blob)
        {
            ShareClient share = new ShareClient(this.attachmentStorageConnectionString, blob.ShareName);
            this.loggerProvider.Info($"Created file share client for share {blob.ShareName}.");

            var folderToUse = blob.FolderName;

            blob.FileType = 3;

            if (blob.MimeType.ToUpperInvariant() == "application/pdf".ToUpperInvariant())
            {
                folderToUse += "/Site Plan";
                blob.FileType = 2;
            }
            else
            {
                folderToUse += "/Evidence";
            }

            var directory = share.GetDirectoryClient(folderToUse);
            var file = directory.GetFileClient(blob.FileName);
            var chunkSize = 4194304; // hard limit in Azure Storage
            long streamPosition = 0;
            long streamLength = 0;
            long bytesRemaining = 0;

            this.loggerProvider.Info($"Obtained file share file reference {file.Path} for file of mime type {blob.MimeType}");

            using (MemoryStream stream = new MemoryStream(this.blobConvertor.Convert(blob)))
            {
                await this.EnsureFoldersExistFor(directory, share).ConfigureAwait(false);

                this.loggerProvider.Info($"Creating file {file.Path}.");
                await file.CreateAsync(stream.Length).ConfigureAwait(false);
                this.loggerProvider.Info($"Created file {file.Path}.");

                streamLength = stream.Length;
                bytesRemaining = streamLength;

                this.loggerProvider.Info($"Creating file content for {file.Path}");
                stream.Position = 0;

                while (streamPosition < streamLength)
                {
                    this.loggerProvider.Info($"Chunk start: {streamPosition}, Bytes remaining: {bytesRemaining}");

                    byte[] buffer = new byte[chunkSize];
                    var bytesRead = await stream.ReadAsync(buffer, 0, (int)chunkSize).ConfigureAwait(false);

                    this.loggerProvider.Info($"{bytesRead} bytes read from stream for current chunk");

                    using (MemoryStream chunkStream = new MemoryStream(buffer.Take(bytesRead).ToArray()))
                    {
                        await file.UploadRangeAsync(new Azure.HttpRange(streamPosition, bytesRead), chunkStream).ConfigureAwait(false);
                    }

                    streamPosition = stream.Position;
                    bytesRemaining -= chunkSize;
                }

                this.loggerProvider.Info($"Created file content for {file.Path}.");
            }

            this.loggerProvider.Info($"Generating file share readonly SAS");

            // now update this blob to have the correct URL.
            // blob.Url = this.GetFileSasUri(blob.ShareName,file.Path,DateTime.MaxValue,this.attachmentStorageAccountName,this.attachmentStorageAccountKey,ShareFileSasPermissions.Read).ToString();
            blob.Url = new UriBuilder($"https://{this.attachmentStorageAccountName}.file.core.windows.net/{blob.ShareName}/{file.Path}").ToString();
            blob.Url = HttpUtility.HtmlEncode(blob.Url);

            this.loggerProvider.Info($"Generated file share readonly SAS");

            return blob;
        }

        /// <summary>
        /// Create a SAS URI for a file.
        /// </summary>
        /// <param name="shareName">The share name being used.</param>
        /// <param name="filePath">The full path to the file including name and extension.</param>
        /// <param name="expiration">How long the uri should last.</param>
        /// <param name="blobAccountName">The storage account name.</param>
        /// <param name="blobAccountKey">The storage account Shared Access Signature key.</param>
        /// <param name="permissions">The file access permissions.</param>
        /// <returns>
        /// An instance of <see cref="Uri"/>.
        /// </returns>
        private Uri GetFileSasUri(
            string shareName,
            string filePath,
            DateTime expiration,
            string blobAccountName,
            string blobAccountKey,
            ShareFileSasPermissions permissions)
        {
            this.loggerProvider.Debug($"Generating SAS Uri for attachment");

            // Get the account details from app settings
            ShareSasBuilder fileSAS = new ShareSasBuilder()
            {
                ShareName = shareName,
                FilePath = filePath,

                // Specify an Azure file resource
                Resource = "f",

                // Expires in 24 hours
                ExpiresOn = expiration,
            };

            // Set the permissions for the SAS
            fileSAS.SetPermissions(permissions);

            // Create a SharedKeyCredential that we can use to sign the SAS token
            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(blobAccountName, blobAccountKey);

            // Build a SAS URI
            UriBuilder fileSasUri = new UriBuilder($"https://{blobAccountName}.file.core.windows.net/{fileSAS.ShareName}/{fileSAS.FilePath}")
            {
                Query = fileSAS.ToSasQueryParameters(credential).ToString(),
            };

            // Return the URI
            return fileSasUri.Uri;
        }

        /// <summary>
        /// Ensures that all of the parent folders for a file actually exist.
        /// </summary>
        /// <param name="directory">The immediate parent folder of the file.</param>
        /// <param name="share">The file share.</param>
        /// <returns>An isntance of <see cref="Task"/>.</returns>
        private async Task EnsureFoldersExistFor(ShareDirectoryClient directory, ShareClient share)
        {
            if (!directory.Exists())
            {
                this.loggerProvider.Info($"'{directory.Path}' does not exist.");
                var folders = directory.Path.Split('/');

                var workingPath = string.Empty;

                foreach (var folder in folders)
                {
                    workingPath += $"{(workingPath.Length == 0 ? string.Empty : "/")}{folder}";
                    this.loggerProvider.Info($"Creating folder '{workingPath}'.");
                    await share.GetDirectoryClient(workingPath)
                        .CreateIfNotExistsAsync()
                        .ConfigureAwait(false);
                    this.loggerProvider.Info($"'{folder}' now exists.");
                }
            }
            else
            {
                this.loggerProvider.Info($"'{directory.Path}' already exists.");
            }
        }

        private string ExtractHandler(string loadHandlerIdentifier)
        {
            string toReturn = null;

            string dataHandlerFileName = string.Format(
                CultureInfo.InvariantCulture,
                ProcessHandlerFileNameFormat,
                loadHandlerIdentifier);

            string dataHandlerPath =
                this.controlHandlersPath + "." + dataHandlerFileName;

            using (Stream stream = this.assembly.GetManifestResourceStream(dataHandlerPath))
            {
                if (stream == null)
                {
                    throw new MissingLoadHandlerFileException(
                        loadHandlerIdentifier);
                }

                using (StreamReader streamReader = new StreamReader(stream))
                {
                    toReturn = streamReader.ReadToEnd();
                }
            }

            return toReturn;
        }
    }
}