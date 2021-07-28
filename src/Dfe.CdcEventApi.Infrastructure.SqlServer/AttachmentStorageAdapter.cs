namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
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
    /// Implements <see cref="IControlStorageAdapter" />.
    /// </summary>
    public class AttachmentStorageAdapter : IAttachmentStorageAdapter
    {
        private const string EXTRACTAttachmentMerge = "EXTRACT-Attachment-Merge";
        private const string EXTRACTAttachmentList = "EXTRACT-Attachment-List";
        private const int CommandTimeoutAsLongAsItTakes = 0;
        private const string ProcessHandlerFileNameFormat = "{0}.sql";
        private readonly string rawDbConnectionString;
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
        public AttachmentStorageAdapter(
            IEntityStorageAdapterSettingsProvider entityStorageAdapterSettingsProvider,
            IBlobConvertor blobConvertor,
            ILoggerProvider loggerProvider)
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

            Type type = typeof(ControlStorageAdapter);

            this.assembly = type.Assembly;

            this.controlHandlersPath = $"{type.Namespace}.AttachmentHandlers";

            this.rawDbConnectionString =
                entityStorageAdapterSettingsProvider.RawDbConnectionString;
        }

        /// <summary>
        /// Creates many <see cref="AttachmentResponse"/> records.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <param name="attachmentStorageConnectionString">The file share connection string.</param>
        /// <param name="attachmentStorageAccountName">The file storage account name.</param>
        /// <param name="attachmentStorageAccountKey">The file storage Shared Access Signature (SAS) key.</param>
        /// <param name="attachments">
        /// A collection of <see cref="AttachmentResponse"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/>.</returns>
        public async Task CreateAsync(
            DateTime runIdentifier,
            string attachmentStorageConnectionString,
            string attachmentStorageAccountName,
            string attachmentStorageAccountKey,
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
            try
            {
                sqlConnection = new SqlConnection(this.rawDbConnectionString);

                sqlConnection.Open();

                this.loggerProvider.Info($"Opened SQL Connection");

                transaction = sqlConnection.BeginTransaction();

                this.loggerProvider.Info($"Started transaction");

                var insertSql = this.ExtractHandler(EXTRACTAttachmentMerge);

                this.loggerProvider.Debug($"Creating Storage Blob records.");

                stopwatch.Start();

                // now update uses of this blob to have the correct URL
                foreach (var blob in attachments)
                {
                    ShareClient share = new ShareClient(attachmentStorageConnectionString, blob.BlobShare);
                    this.loggerProvider.Info($"Created file share client for share {blob.BlobShare}.");

                    var folderToUse = blob.BlobFolder;

                    if (blob.BlobMimeType.ToUpperInvariant() == "application/pdf".ToUpperInvariant())
                    {
                        folderToUse += "/Site Plan";
                    }
                    else
                    {
                        folderToUse += "/Evidence";
                    }

                    var directory = share.GetDirectoryClient(folderToUse);
                    var file = directory.GetFileClient(blob.BlobFilename);

                    this.loggerProvider.Info($"Obtained file share file reference {file.Path} for file of mime type {blob.BlobMimeType}.");

                    using (MemoryStream stream = new MemoryStream(this.blobConvertor.Convert(blob)))
                    {
                        await this.EnsureFoldersExistFor(directory, share).ConfigureAwait(false);

                        this.loggerProvider.Info($"Creating file {file.Path}.");
                        await file.CreateAsync(stream.Length).ConfigureAwait(false);
                        this.loggerProvider.Info($"Created file {file.Path}.");

                        this.loggerProvider.Info($"Creating file content for {file.Path}.");
                        stream.Position = 0;
                        await file.UploadRangeAsync(new HttpRange(0, stream.Length), stream).ConfigureAwait(false);
                        this.loggerProvider.Info($"Created file content for {file.Path}.");
                    }

                    this.loggerProvider.Info($"Generating file share readonly SAS");

                    blob.BlobUrl = GetFileSasUri(
                        blob.BlobShare,
                        file.Path,
                        DateTime.MaxValue,
                        attachmentStorageAccountName,
                        attachmentStorageAccountKey,
                        ShareFileSasPermissions.Read).ToString();

                    this.loggerProvider.Info($"Generated file share readonly SAS");

                    this.loggerProvider.Info($"Storing blob key 'obtained' and metadata references");

                    // add it as a known blob
                    await sqlConnection
                        .ExecuteAsync(insertSql, blob, transaction, CommandTimeoutAsLongAsItTakes)
                        .ConfigureAwait(false);

                    this.loggerProvider.Info($"Stored Blob Key 'obtained' and metadata references");
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
                    this.loggerProvider.Info($"Closing database connection.");
                    sqlConnection?.Close();
                }

                sqlConnection?.Dispose();
            }
        }

        /// <inheritdoc/>
        public Task<IEnumerable<AttachmentRequest>> GetAsync()
        {
            this.loggerProvider.Info($"Getting requried attachments list.");

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
        private static Uri GetFileSasUri(
            string shareName,
            string filePath,
            DateTime expiration,
            string blobAccountName,
            string blobAccountKey,
            ShareFileSasPermissions permissions)
        {
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