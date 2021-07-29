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
    /// Implements <see cref="IControlStorageAdapter" />.
    /// </summary>
    public class ControlStorageAdapter : IControlStorageAdapter
    {
        private const string ControlCreate = "CONTROL-Create";
        private const string CONTROLGet = "CONTROL-Get";
        private const string CONTROLGetCount = "CONTROL-Get-Count";
        private const string CONTROLSince = "CONTROL-Since";
        private const string CONTROLStatusUpdate = "CONTROL-Status-Update";
        private const string CONTROLUpdate = "CONTROL-Update";
        private const string ProcessHandlerFileNameFormat = "{0}.sql";
        private const int CommandTimeoutAsLongAsItTakes = 0;
        private readonly ILoggerProvider loggerProvider;
        private readonly Assembly assembly;
        private readonly string controlHandlersPath;
        private readonly string rawDbConnectionString;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="ControlStorageAdapter" /> class.
        /// </summary>
        /// <param name="entityStorageAdapterSettingsProvider">
        /// An instance of type
        /// <see cref="IEntityStorageAdapterSettingsProvider" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public ControlStorageAdapter(
            IEntityStorageAdapterSettingsProvider entityStorageAdapterSettingsProvider,
            ILoggerProvider loggerProvider)
        {
            if (entityStorageAdapterSettingsProvider == null)
            {
                throw new ArgumentNullException(
                    nameof(entityStorageAdapterSettingsProvider));
            }

            this.loggerProvider = loggerProvider ?? throw new ArgumentNullException(
                   nameof(loggerProvider));

            Type type = typeof(ControlStorageAdapter);

            this.assembly = type.Assembly;

            this.controlHandlersPath = $"{type.Namespace}.ControlHandlers";

            this.rawDbConnectionString =
                entityStorageAdapterSettingsProvider.RawDbConnectionString;
        }

        /// <summary>
        /// Starts the load by creating and returning a new <see cref="Control"/> model.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an instance of <see cref="Control"/> of the run start.
        /// </returns>
        public async Task<IEnumerable<Control>> CreateAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Starting a control instance at {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                var insertSql = this.ExtractHandler(ControlCreate);
                this.loggerProvider.Debug($"Creating control record.");

                stopwatch.Start();

                await sqlConnection
                        .ExecuteAsync(insertSql, new { runIdentifier }, null, CommandTimeoutAsLongAsItTakes)
                        .ConfigureAwait(false);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Insert executed with success, time elapsed: " +
                    $"{elapsed}.");

                this.loggerProvider.Debug($"Retrieving this current and last successful control records.");

                string querySql = this.ExtractHandler(CONTROLSince);

                stopwatch.Restart();

                IEnumerable<Control> controls = sqlConnection.Query<Control>(
                    querySql,
                    new { runIdentifier },
                    null,
                    true,
                    CommandTimeoutAsLongAsItTakes);

                stopwatch.Stop();

                elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Query executed with success, time elapsed: " +
                    $"{elapsed}.");

                return controls;
            }
        }

        /// <summary>
        /// Gets the <see cref="Control"/> for the specified date and time.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="Control"/> of the run.
        /// </returns>
        public Task<Control> GetAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting a load from {runIdentifier:O}");

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                string querySql = this.ExtractHandler(CONTROLGet);
                this.loggerProvider.Debug($"Retrieving Load record.");

                stopwatch.Start();

                Control load = sqlConnection.Query<Control>(
                                    querySql,
                                    new { runIdentifier },
                                    null,
                                    true,
                                    CommandTimeoutAsLongAsItTakes)
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
        public Task<int> GetCountAsync(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Getting the count of loaded rows for {runIdentifier}");

            string querySql = this.ExtractHandler(CONTROLGetCount);

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Retrieving Load count.");

                stopwatch.Start();

                var results = sqlConnection.Query<int>(querySql, new { runIdentifier }, null, true, CommandTimeoutAsLongAsItTakes)
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
        /// Updates a <see cref="Control"/> of the specified date and time.
        /// </summary>
        /// <param name="item">
        /// The new version of the <see cref="Control"/> item.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        public async Task UpdateAsync(Control item)
        {
            if (item == null)
            {
                throw new ArgumentNullException($"{nameof(item)}");
            }

            this.loggerProvider.Info($"Updating a load from {item.Load_DateTime:O}");

            string udpateSql = this.ExtractHandler(CONTROLUpdate);
            SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.DateTime);

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Updating Load record.");

                stopwatch.Start();

                await sqlConnection.ExecuteAsync(udpateSql, item, null, CommandTimeoutAsLongAsItTakes)
                    .ConfigureAwait(false);

                stopwatch.Stop();

                TimeSpan elapsed = stopwatch.Elapsed;

                this.loggerProvider.Info(
                    $"Update executed with success, time elapsed: " +
                    $"{elapsed}.");
            }
        }

        /// <summary>
        /// Updates the status of a <see cref="Control"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The identifier of the <see cref="Control"/>.
        /// </param>
        /// <param name="status">
        /// The valof the new status.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        public async Task UpdateStatusAsync(DateTime runIdentifier, int status)
        {
            this.loggerProvider.Info($"Updating load at {runIdentifier:O} to {status}");

            var updateSql = this.ExtractHandler(CONTROLStatusUpdate);

            Stopwatch stopwatch = new Stopwatch();
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug($"Updating Load record status.");

                var state = status.ToEnum<ControlState>();

                stopwatch.Start();

                await sqlConnection.ExecuteAsync(
                    updateSql,
                    new
                    {
                        runIdentifier,
                        status,
                        reportTitle = $"Current step: {state}",
                    },
                    null,
                    CommandTimeoutAsLongAsItTakes)
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