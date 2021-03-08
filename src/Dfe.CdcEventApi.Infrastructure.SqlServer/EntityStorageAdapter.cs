namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System;
    using System.Data;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Definitions;

    /// <summary>
    /// Implements <see cref="IEntityStorageAdapter" />.
    /// </summary>
    public class EntityStorageAdapter : IEntityStorageAdapter
    {
        private const string DataHandlerFileNameFormat = "{0}.sql";

        private readonly Assembly assembly;
        private readonly string dataHandlersPath;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="EntityStorageAdapter" /> class.
        /// </summary>
        public EntityStorageAdapter()
        {
            Type type = typeof(EntityStorageAdapter);

            this.assembly = type.Assembly;

            this.dataHandlersPath = $"{type.Namespace}.DataHandlers";
        }

        /// <inheritdoc />
        public async Task StoreEntitiesAsync(
            string dataHandlerIdentifier,
            DataTable dataTable,
            CancellationToken cancellationToken)
        {
            // 1) Extract the data handler, via the identifier.
            string handlerScript = this.ExtractHandler(dataHandlerIdentifier);

            // TODO:
            // 2) Execute the SQL.
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
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    toReturn = streamReader.ReadToEnd();
                }
            }

            return toReturn;
        }
    }
}