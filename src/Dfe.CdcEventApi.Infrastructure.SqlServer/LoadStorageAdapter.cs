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
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Implements <see cref="ILoadStorageAdapter" />.
    /// </summary>
    public class LoadStorageAdapter : ILoadStorageAdapter
    {
        private const string DataHandlerFileNameFormat = "{0}.sql";
        private readonly ILoggerProvider loggerProvider;
        private readonly Assembly assembly;
        private readonly string dataHandlersPath;
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

            this.dataHandlersPath = $"{type.Namespace}.DataHandlers";

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
            this.loggerProvider.Info($"Starting a load at {runIdentifier:u}");

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
        public Task<Load> GetLoadAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting a load from {runIdentifier:u}");

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

            this.loggerProvider.Info($"Updating a load from {item.Load_DateTime:u}");

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
            this.loggerProvider.Info($"Updating load at {runIdentifier:u} to {status}");

            var updateSql = "UPDATE [dbo].[Raw_Load]  SET [Status] = @status WHERE [Load_DateTime]= @runIdentifier";

            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Updating Load record.");

                var count = await sqlConnection.ExecuteAsync(updateSql, new { runIdentifier, status })
                        .ConfigureAwait(false);
            }
        }

        private string ExtractHandler(string loadHandlerIdentifier)
        {
            string toReturn = null;

            string dataHandlerFileName = string.Format(
                CultureInfo.InvariantCulture,
                DataHandlerFileNameFormat,
                loadHandlerIdentifier);

            string dataHandlerPath =
                this.dataHandlersPath + "." + dataHandlerFileName;

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