namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
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

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                var insertSql = this.ExtractHandler("Create_Raw_Load");
                this.loggerProvider.Debug($"Creating Load record.");
                var count = await sqlConnection
                        .ExecuteAsync(insertSql, new { runIdentifier })
                        .ConfigureAwait(false);

                this.loggerProvider.Debug($"Retrieving new Load record.");
                string querySql = this.ExtractHandler("Retrieve_Raw_LoadSince");
                IEnumerable<Load> loads = sqlConnection
                        .Query<Load>(querySql, new { runIdentifier });
                return loads;
            }
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <returns>...</returns>
        public Task<IEnumerable<Attachment>> GetAttachments(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting attachments from {runIdentifier:O}");

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                string querySql = this.ExtractHandler("Retrieve_Raw_Load_Attachments");
                this.loggerProvider.Debug($"Retrieving attachment records.");
                var attachments = sqlConnection.Query<Attachment>(querySql, new { runIdentifier });
                return Task.FromResult(attachments);
            }
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <returns>...</returns>
        public Task<Load> GetLoadAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting a load from {runIdentifier:O}");

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                string querySql = this.ExtractHandler("Retrieve_Raw_Load");
                this.loggerProvider.Debug($"Retrieving Load record.");
                Load load = sqlConnection.Query<Load>(querySql, new { runIdentifier })
                        .FirstOrDefault();
                return Task.FromResult(load);
            }
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="status">..</param>
        /// <returns>...</returns>
        public Task<IEnumerable<LoadNotification>> GetLoadNotificationsForStatus(short status)
        {
            this.loggerProvider.Info($"Getting all notifications for status of {status}");

            string querySql = this.ExtractHandler("Retrieve_Raw_LoadNotification");

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Retrieving new Load record.");
                var results = sqlConnection.Query<LoadNotification>(querySql, new { status });
                return Task.FromResult(results);
            }
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="status">..</param>
        /// <returns>...</returns>
        public Task<LoadNotificationTemplate> GetLoadTemplateForStatus(short status)
        {
            this.loggerProvider.Info($"Getting template for the load notification for status of {status}");

            string querySql = this.ExtractHandler("Retrieve_Raw_LoadNotificationTemplate");

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Retrieving new Load Notification Template record.");
                var results = sqlConnection.Query<LoadNotificationTemplate>(querySql, new { status }).FirstOrDefault();
                return Task.FromResult(results);
            }
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="item">..</param>
        /// <returns>...</returns>
        public async Task UpdateLoadAsync(Load item)
        {
            if (item == null)
            {
                throw new ArgumentNullException($"{nameof(item)}");
            }

            this.loggerProvider.Info($"Updating a load from {item.Load_DateTime:O}");

            string udpateSql = this.ExtractHandler("Update_Raw_Load");

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Updating Load record.");
                await sqlConnection.ExecuteAsync(udpateSql, item)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <param name="status">...</param>
        /// <returns>....</returns>
        public async Task UpdateLoadStatusAsync(DateTime runIdentifier, short status)
        {
            this.loggerProvider.Info($"Updating load at {runIdentifier:O} to {status}");

            var updateSql = this.ExtractHandler("Update_Raw_LoadStatus");

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Updating Load record status.");

                var state = status.ToEnum<LoadStates>();

                await sqlConnection.ExecuteAsync(updateSql, new { runIdentifier, status, reportTitle = $"Current step: {state}" })
                        .ConfigureAwait(false);
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