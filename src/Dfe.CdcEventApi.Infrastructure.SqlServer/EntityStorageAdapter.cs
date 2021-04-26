namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Dfe.CdcEventApi.Domain.Exceptions;

    /// <summary>
    /// Implements <see cref="IEntityStorageAdapter" />.
    /// </summary>
    public class EntityStorageAdapter : IEntityStorageAdapter
    {
        private const int DefaultCommandTimeout = 300;

        private const string DataHandlerFileNameFormat = "{0}.sql";

        private readonly ILoggerProvider loggerProvider;

        private readonly Assembly assembly;
        private readonly string dataHandlersPath;
        private readonly string rawDbConnectionString;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="EntityStorageAdapter" /> class.
        /// </summary>
        /// <param name="entityStorageAdapterSettingsProvider">
        /// An instance of type
        /// <see cref="IEntityStorageAdapterSettingsProvider" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public EntityStorageAdapter(
            IEntityStorageAdapterSettingsProvider entityStorageAdapterSettingsProvider,
            ILoggerProvider loggerProvider)
        {
            if (entityStorageAdapterSettingsProvider == null)
            {
                throw new ArgumentNullException(
                    nameof(entityStorageAdapterSettingsProvider));
            }

            this.loggerProvider = loggerProvider;

            Type type = typeof(EntityStorageAdapter);

            this.assembly = type.Assembly;

            this.dataHandlersPath = $"{type.Namespace}.DataHandlers";

            this.rawDbConnectionString =
                entityStorageAdapterSettingsProvider.RawDbConnectionString;
        }

        /// <inheritdoc />
        public async Task StoreEntitiesAsync(
            string dataHandlerIdentifier,
            DateTime runIdentifier,
            XDocument xDocument,
            CancellationToken cancellationToken)
        {
            if (xDocument == null)
            {
                throw new ArgumentNullException(nameof(xDocument));
            }

            // 1) Extract the data handler, via the identifier.
            this.loggerProvider.Debug(
                $"Extracting data handler with identifier " +
                $"\"{dataHandlerIdentifier}\"...");

            string handlerScript = this.ExtractHandler(dataHandlerIdentifier);

            this.loggerProvider.Info(
                $"Handler script for identifier \"{dataHandlerIdentifier}\" " +
                $"extracted.");

            // 2) Execute the SQL.
            using (SqlConnection sqlConnection = new SqlConnection(this.rawDbConnectionString))
            {
                this.loggerProvider.Debug(
                    $"Opening {nameof(sqlConnection)}...");

                await sqlConnection.OpenAsync(cancellationToken)
                    .ConfigureAwait(false);

                this.loggerProvider.Info(
                    $"{nameof(sqlConnection)} opened with success " +
                    $"({nameof(sqlConnection.ClientConnectionId)}: " +
                    $"{sqlConnection.ClientConnectionId}).");

                Stopwatch stopwatch = new Stopwatch();
                using (SqlCommand sqlCommand = this.GetSqlCommand(sqlConnection, handlerScript, runIdentifier, xDocument))
                {
                    this.loggerProvider.Debug("Executing query...");

                    stopwatch.Start();

                    await sqlCommand.ExecuteNonQueryAsync(cancellationToken)
                        .ConfigureAwait(false);

                    stopwatch.Stop();

                    TimeSpan elapsed = stopwatch.Elapsed;

                    this.loggerProvider.Info(
                        $"Query executed with success, time elapsed: " +
                        $"{elapsed}.");
                }
            }
        }

        [SuppressMessage(
            "Microsoft.Security",
            "CA2100",
            Justification = "Does not contain any user input.")]
        private SqlCommand GetSqlCommand(
            SqlConnection sqlConnection,
            string handlerScript,
            DateTime runIdentifier,
            XDocument xDocument)
        {
            SqlCommand toReturn = null;

            CommandType commandType = CommandType.Text;

            this.loggerProvider.Debug(
                $"Preparing {nameof(SqlCommand)} with query " +
                $"\"{handlerScript}\" and {nameof(commandType)} = " +
                $"{commandType}...");

            toReturn = new SqlCommand(handlerScript, sqlConnection)
            {
                CommandType = commandType,
            };

            this.loggerProvider.Debug(
                $"Preparing {nameof(SqlParameter)} using " +
                $"{nameof(xDocument)}...");

            string xDocumentStr = xDocument.ToString();

            SqlParameter sqlParameter = new SqlParameter(
                "Entities",
                xDocumentStr)
            {
                DbType = DbType.Xml,
            };

            toReturn.Parameters.Add(sqlParameter);

            sqlParameter = new SqlParameter(
                "RunIdentifier",
                runIdentifier)
            {
                DbType = DbType.DateTime,
            };

            toReturn.Parameters.Add(sqlParameter);

            toReturn.CommandTimeout = DefaultCommandTimeout;

            return toReturn;
        }

        private string ExtractHandler(string dataHandlerIdentifier)
        {
            string toReturn = null;

            string dataHandlerFileName = string.Format(
                CultureInfo.InvariantCulture,
                DataHandlerFileNameFormat,
                dataHandlerIdentifier);

            string dataHandlerPath =
                this.dataHandlersPath + "." + dataHandlerFileName;

            using (Stream stream = this.assembly.GetManifestResourceStream(dataHandlerPath))
            {
                if (stream == null)
                {
                    throw new MissingDataHandlerFileException(
                        dataHandlerIdentifier);
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