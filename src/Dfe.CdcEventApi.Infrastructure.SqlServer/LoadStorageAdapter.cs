namespace Dfe.CdcEventApi.Infrastructure.SqlServer
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
    using Dapper;
    using Dfe.CdcEventApi.Domain;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Implements <see cref="ILoadStorageAdapter" />.
    /// </summary>
    public class LoadStorageAdapter : ILoadStorageAdapter
    {
        private const string LoadHandlerFileNameFormat = "{0}.sql";
        private readonly ILoggerProvider loggerProvider;
        private readonly Assembly assembly;
        private readonly string loadHandlersPath;
        private readonly string rawDbConnectionString;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="LoadStorageAdapter" /> class.
        /// </summary>
        /// <param name="entityStorageAdapterSettingsProvider">
        /// An instance of type
        /// <see cref="IEntityStorageAdapterSettingsProvider" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public LoadStorageAdapter(
            IEntityStorageAdapterSettingsProvider entityStorageAdapterSettingsProvider,
            ILoggerProvider loggerProvider)
        {
            if (entityStorageAdapterSettingsProvider == null)
            {
                throw new ArgumentNullException(
                    nameof(entityStorageAdapterSettingsProvider));
            }

            this.loggerProvider = loggerProvider;

            Type type = typeof(LoadStorageAdapter);

            this.assembly = type.Assembly;

            this.loadHandlersPath = $"{type.Namespace}.LoadHandlers";

            this.rawDbConnectionString =
                entityStorageAdapterSettingsProvider.RawDbConnectionString;
        }

        /// <summary>
        /// Creates many <see cref="Blob"/> records.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <param name="blobs">
        /// A collection of <see cref="Blob"/>.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/>.</returns>
        public async Task CreateBlobsAsync(DateTime runIdentifier, IEnumerable<Blob> blobs)
        {
            if (blobs == null)
            {
                throw new ArgumentNullException(nameof(blobs));
            }

            this.loggerProvider.Info($"Starting a blob load at {runIdentifier:O}");

            this.loggerProvider.Info($"Converting blob records to storage blob records.");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                var insertSql = this.ExtractHandler("Create_Extract_Blob");
                var updateSql = this.ExtractHandler("Update_Extract_Attachment-Uses");
                this.loggerProvider.Debug($"Creating Storage Blob records.");

                stopwatch.Start();

                await sqlConnection
                        .ExecuteAsync(insertSql, blobs)
                        .ConfigureAwait(false);

                // now update uses of this blob to have the correct URL
                foreach (var blob in blobs)
                {
                    await sqlConnection
                        .ExecuteAsync(updateSql, blob)
                        .ConfigureAwait(false);
                }

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");
            }
        }

        /// <summary>
        /// Starts the load by creating and returning a new <see cref="Load"/> model.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an instance of <see cref="Load"/> of the run start.
        /// </returns>
        public async Task<IEnumerable<Load>> CreateLoadAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Starting a load at {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                var insertSql = this.ExtractHandler("Create_Raw_Load");
                this.loggerProvider.Debug($"Creating Load record.");

                stopwatch.Start();

                await sqlConnection
                        .ExecuteAsync(insertSql, new { runIdentifier })
                        .ConfigureAwait(false);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Insert executed with success, time elapsed: " +
                    $"{elapsed}.");

                this.loggerProvider.Debug($"Retrieving this current and last successful load records.");

                string querySql = this.ExtractHandler("Retrieve_Raw_LoadSince");

                stopwatch.Restart();

                IEnumerable<Load> loads = sqlConnection.Query<Load>(
                    querySql,
                    new { runIdentifier });

                stopwatch.Stop();

                elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return loads;
            }
        }

        /// <summary>
        /// Execute the extract process.
        /// </summary>
        /// <param name="runIdentifier">The date and time of the run.</param>
        /// <returns>An <see cref="Task"/> .</returns>
        public async Task ExecuteExtract(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Starting an extract at {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                var procedureSql = this.ExtractHandler("Execute_Extract");

                this.loggerProvider.Debug($"Executing the extract.");

                stopwatch.Start();

                int commandTimeoutAsLongAsItTakes = 0;

                await sqlConnection
                        .ExecuteAsync(procedureSql, new { runIdentifier }, null, commandTimeoutAsLongAsItTakes)
                        .ConfigureAwait(false);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Extract executed with success, time elapsed: " +
                    $"{elapsed}.");
            }
        }

        /// <summary>
        /// Execute the transform process.
        /// </summary>
        /// <param name="runIdentifier">The date and time of the run.</param>
        /// <returns>An <see cref="Task"/> .</returns>
        public async Task ExecuteTransform(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Starting a transform at {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                var procedureSql = this.ExtractHandler("Execute_Transform");

                this.loggerProvider.Debug($"Executing the transform.");

                stopwatch.Start();

                int commandTimeoutAsLongAsItTakes = 0;

                await sqlConnection
                        .ExecuteAsync(procedureSql, new { runIdentifier }, null, commandTimeoutAsLongAsItTakes)
                        .ConfigureAwait(false);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Transform executed with success, time elapsed: " +
                    $"{elapsed}.");
            }
        }

        /// <summary>
        /// Gets the Attechment process instruction records for the current load.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="Attachment"/> of the run.
        /// </returns>
        public Task<IEnumerable<Attachment>> GetAttachments(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting attachments from {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                string querySql = this.ExtractHandler("Retrieve_Extract_Attachments");
                this.loggerProvider.Debug($"Retrieving attachment records.");

                stopwatch.Start();

                var attachments = sqlConnection.Query<Attachment>(querySql, new { runIdentifier });

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return Task.FromResult(attachments);
            }
        }

        /// <summary>
        /// Gets the <see cref="Load"/> for the specified date and time.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="Load"/> of the run.
        /// </returns>
        public Task<Load> GetLoadAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting a load from {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                string querySql = this.ExtractHandler("Retrieve_Raw_Load");
                this.loggerProvider.Debug($"Retrieving Load record.");

                stopwatch.Start();

                Load load = sqlConnection.Query<Load>(
                                    querySql,
                                    new { runIdentifier })
                                    .FirstOrDefault();

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return Task.FromResult(load);
            }
        }

        /// <summary>
        /// Gets the loaded row count for the run.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        public Task<int> GetLoadCountAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting the count of loaded rows for {runIdentifier}");

            string querySql = this.ExtractHandler("Retrieve_Raw_LoadCount");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Retrieving Load count.");

                stopwatch.Start();

                var results = sqlConnection.Query<int>(querySql, new { runIdentifier })
                                                .FirstOrDefault();

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return Task.FromResult(results);
            }
        }

        /// <summary>
        /// Gets the collection of notifications for the status of any load.
        /// </summary>
        /// <param name="status">
        /// The status value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="LoadNotification"/>.
        /// </returns>
        public Task<IEnumerable<LoadNotification>> GetLoadNotificationsForStatus(short status)
        {
            this.loggerProvider.Info($"Getting all notifications for status of {status}");

            string querySql = this.ExtractHandler("Retrieve_Raw_LoadNotification");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Retrieving new Load record.");

                stopwatch.Start();

                var results = sqlConnection.Query<LoadNotification>(querySql, new { status });

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return Task.FromResult(results);
            }
        }

        /// <summary>
        /// Gets the report template for the specified status of any load.
        /// </summary>
        /// <param name="status">
        /// The status value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="LoadNotificationTemplate"/>.
        /// </returns>
        public Task<LoadNotificationTemplate> GetLoadTemplateForStatus(short status)
        {
            this.loggerProvider.Info($"Getting template for the load notification for status of {status}");

            string querySql = this.ExtractHandler("Retrieve_Raw_LoadNotificationTemplate");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Retrieving new load notification template record.");

                stopwatch.Start();

                var results = sqlConnection.Query<LoadNotificationTemplate>(
                                                querySql,
                                                new { status })
                                            .FirstOrDefault();

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return Task.FromResult(results);
            }
        }

        /// <summary>
        /// Updates a <see cref="Load"/> of the specified date and time.
        /// </summary>
        /// <param name="item">
        /// The new version of the <see cref="Load"/> item.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        public async Task UpdateLoadAsync(Load item)
        {
            if (item == null)
            {
                throw new ArgumentNullException($"{nameof(item)}");
            }

            this.loggerProvider.Info($"Updating a load from {item.Load_DateTime:O}");

            string udpateSql = this.ExtractHandler("Update_Raw_Load");
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime);

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Updating Load record.");

                stopwatch.Start();

                await sqlConnection.ExecuteAsync(udpateSql, item)
                    .ConfigureAwait(false);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Update executed with success, time elapsed: " +
                    $"{elapsed}.");
            }
        }

        /// <summary>
        /// Updates the status of a <see cref="Load"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The identifier of the <see cref="Load"/>.
        /// </param>
        /// <param name="status">
        /// The valof the new status.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        public async Task UpdateLoadStatusAsync(DateTime runIdentifier, short status)
        {
            this.loggerProvider.Info($"Updating load at {runIdentifier:O} to {status}");

            var updateSql = this.ExtractHandler("Update_Raw_LoadStatus");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Updating Load record status.");

                var state = status.ToEnum<LoadStates>();

                stopwatch.Start();

                await sqlConnection.ExecuteAsync(
                    updateSql,
                    new
                    {
                        runIdentifier,
                        status,
                        reportTitle = $"Current step: {state}",
                    })
                        .ConfigureAwait(false);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Update executed with success, time elapsed: " +
                    $"{elapsed}.");
            }
        }

        private string ExtractHandler(string loadHandlerIdentifier)
        {
            string toReturn = null;

            string dataHandlerFileName = string.Format(
                CultureInfo.InvariantCulture,
                LoadHandlerFileNameFormat,
                loadHandlerIdentifier);

            string dataHandlerPath =
                this.loadHandlersPath + "." + dataHandlerFileName;

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