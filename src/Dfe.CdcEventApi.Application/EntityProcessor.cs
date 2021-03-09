namespace Dfe.CdcEventApi.Application
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Exceptions;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Implements <see cref="IEntityProcessor" />.
    /// </summary>
    public class EntityProcessor : IEntityProcessor
    {
        private readonly IEntityStorageAdapter entityStorageAdapter;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityProcessor" />
        /// class.
        /// </summary>
        /// <param name="entityStorageAdapter">
        /// An instance of type <see cref="IEntityStorageAdapter" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public EntityProcessor(
            IEntityStorageAdapter entityStorageAdapter,
            ILoggerProvider loggerProvider)
        {
            this.entityStorageAdapter = entityStorageAdapter;
            this.loggerProvider = loggerProvider;
        }

        /// <inheritdoc />
        public async Task ProcessEntitiesAsync<TModelsBase>(
            IEnumerable<TModelsBase> modelsBases,
            CancellationToken cancellationToken)
            where TModelsBase : ModelsBase
        {
            // 1) Figure out which embedded TSQL script to invoke from the
            //    meta-data of TModels base.
            string identifier = ExtractDataHandlerIdentifier<TModelsBase>();

            // 2) Prepare the main XDocument to be passed to the data-layer.
            XDocument xDocument = this.ConvertToXDocument(modelsBases);

            // 3) Invoke the data-layer with the script and the DataTable.
            await this.entityStorageAdapter.StoreEntitiesAsync(
                identifier,
                xDocument,
                cancellationToken)
                .ConfigureAwait(false);

            // TODO:
            // 4) Recursively check the model meta-data for embedded
            //    collections.
        }

        private static string ExtractDataHandlerIdentifier<TModelsBase>()
            where TModelsBase : ModelsBase
        {
            string toReturn = null;

            Type entityType = typeof(TModelsBase);
            Type attributeType = typeof(DataHandlerAttribute);

            DataHandlerAttribute dataHandlerAttribute =
                (DataHandlerAttribute)Attribute.GetCustomAttribute(
                    entityType,
                    attributeType);

            if (dataHandlerAttribute == null)
            {
                throw new MissingDataHandlerAttributeException(entityType);
            }

            toReturn = dataHandlerAttribute.Identifier;

            return toReturn;
        }

        private XDocument ConvertToXDocument(
            IEnumerable<ModelsBase> modelsBases)
        {
            XDocument toReturn = null;

            IEnumerable<IDictionary<string, JToken>> datas = modelsBases
                .Select(x => x.Data);

            string[] distinctColumnNames = datas
                .SelectMany(x => x.Keys)
                .Distinct()
                .ToArray();

            string distinctColumnNamesList = string.Join(
                ", ",
                distinctColumnNames);

            this.loggerProvider.Debug(
                $"{nameof(distinctColumnNamesList)} = " +
                $"\"{distinctColumnNames}\"");

            string dataTableXmlStr = null;
            using (DataTable dataTable = new DataTable("Entity"))
            {
                DataColumn[] dataColumns = distinctColumnNames
                    .Select(x => new DataColumn(x))
                    .ToArray();

                dataTable.Columns.AddRange(dataColumns);

                this.loggerProvider.Debug(
                    $"{dataColumns.Length} {nameof(DataColumn)}s added to " +
                    $"{nameof(dataTable)}.");

                DataRow dataRow = null;
                foreach (IDictionary<string, JToken> data in datas)
                {
                    dataRow = dataTable.NewRow();

                    foreach (KeyValuePair<string, JToken> keyValuePair in data)
                    {
                        dataRow[keyValuePair.Key] = keyValuePair.Value;
                    }

                    dataTable.Rows.Add(dataRow);
                }

                this.loggerProvider.Debug(
                    $"{dataTable.Rows.Count} row(s) appended to " +
                    $"{nameof(dataTable)}.");

                this.loggerProvider.Debug(
                    $"Transforming {nameof(dataTable)} to " +
                    $"{nameof(XDocument)}...");

                using (StringWriter stringWriter = new StringWriter())
                {
                    dataTable.WriteXml(stringWriter);

                    dataTableXmlStr = stringWriter.ToString();
                }
            }

            toReturn = XDocument.Parse(dataTableXmlStr);

            return toReturn;
        }
    }
}