namespace Dfe.CdcEventApi.Application
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
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
        public async Task CreateEntitiesAsync<TModelsBase>(
            DateTime runIdentifier,
            IEnumerable<TModelsBase> modelsBases,
            CancellationToken cancellationToken)
            where TModelsBase : ModelsBase
        {
            if (modelsBases == null)
            {
                throw new ArgumentNullException(nameof(modelsBases));
            }

            // 1) Figure out which embedded TSQL script to invoke from the
            //    meta-data of TModels base for this verb action.
            string identifier = ExtractDataHandlerIdentifier<TModelsBase>("Post");

            if (string.IsNullOrWhiteSpace(identifier))
            {
                throw new MissingDataHandlerAttributeException(typeof(TModelsBase));
            }

            // 2) Prepare the main XDocument to be passed to the data-layer.
            XDocument xDocument = this.ConvertToXDocument(modelsBases);

            // 3) Invoke the data-layer with the script and the DataTable.
            await this.entityStorageAdapter.StoreEntitiesAsync(
                identifier,
                runIdentifier,
                xDocument,
                cancellationToken)
                .ConfigureAwait(false);

            // 4) Identify any sub-collection properties for DataHandler
            //    identifiers for this verb action.
            Dictionary<PropertyInfo, string> propertiesToProcess =
                ExtractPropertyInfosAndDataHanderIdentifiers<TModelsBase>("Post");

            this.loggerProvider.Debug(
                $"This entity has {propertiesToProcess.Count} children to " +
                $"also process.");

            foreach (TModelsBase modelsBase in modelsBases)
            {
                foreach (KeyValuePair<PropertyInfo, string> propertyToProcess in propertiesToProcess)
                {
                    await this.ProcessProperty(
                        runIdentifier,
                        modelsBase,
                        propertyToProcess,
                        cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }

        private static Dictionary<PropertyInfo, string> ExtractPropertyInfosAndDataHanderIdentifiers<TModelsBase>(string forVerb)
            where TModelsBase : ModelsBase
        {
            Dictionary<PropertyInfo, string> toReturn = null;

            Type entityType = typeof(TModelsBase);
            Type attributeType = typeof(DataHandlerAttribute);

            toReturn = entityType
                .GetProperties()
                .Select(x =>
                {
                    string identifier = null;

                    DataHandlerAttribute dataHandlerAttribute =
                        (DataHandlerAttribute)x.GetCustomAttributes(attributeType).FirstOrDefault(a => ((DataHandlerAttribute)a).Verb == forVerb);

                    if (dataHandlerAttribute != null)
                    {
                        identifier = dataHandlerAttribute.Identifier;
                    }

                    return new
                    {
                        PropertyInfo = x,
                        Identifier = identifier,
                    };
                })
                .Where(x => !string.IsNullOrEmpty(x.Identifier))
                .ToDictionary(x => x.PropertyInfo, x => x.Identifier);

            return toReturn;
        }

        private static string ExtractDataHandlerIdentifier<TModelsBase>(string forVerb)
            where TModelsBase : ModelsBase
        {
            string toReturn = null;

            Type entityType = typeof(TModelsBase);
            Type attributeType = typeof(DataHandlerAttribute);

            DataHandlerAttribute[] dataHandlerAttributes =
                Attribute.GetCustomAttributes(
                    entityType,
                    attributeType) as DataHandlerAttribute[];

            if (dataHandlerAttributes == null)
            {
                throw new MissingDataHandlerAttributeException(entityType);
            }

            toReturn = dataHandlerAttributes.FirstOrDefault(a => a.Verb == forVerb)?.Identifier;

            return toReturn;
        }

        private async Task ProcessProperty(
            DateTime runIdentifier,
            ModelsBase modelsBase,
            KeyValuePair<PropertyInfo, string> propertyToProcess,
            CancellationToken cancellationToken)
        {
            // 1) Pull back the value for this model, and this
            //    property.
            PropertyInfo propertyInfo = propertyToProcess.Key;
            string identifier = propertyToProcess.Value;

            this.loggerProvider.Debug(
                $"Extracting property \"{propertyInfo.Name}\"...");

            object propertyValue = propertyInfo.GetValue(modelsBase);

            // 2) Should be a collection of models inheriting from
            //    ModelsBase.
            //    If it's not, we'll need to implement it. For now,
            //    no point in gold-plating.
            IEnumerable<ModelsBase> subCollection = null;

            if (propertyValue != null)
            {
                subCollection = (IEnumerable<ModelsBase>)propertyValue;
            }
            else
            {
                // If the property isn't specified.
                subCollection = Array.Empty<ModelsBase>();
            }

            this.loggerProvider.Info(
                $"{subCollection.Count()} {nameof(ModelsBase)}s extracted. " +
                $"Converting to {nameof(XDocument)}...");

            XDocument xDocument = this.ConvertToXDocument(subCollection);

            this.loggerProvider.Debug(
                $"Storing {subCollection.Count()} entities using data " +
                $"handler identifier \"{identifier}\"...");

            await this.entityStorageAdapter.StoreEntitiesAsync(
                identifier,
                runIdentifier,
                xDocument,
                cancellationToken)
                .ConfigureAwait(false);

            this.loggerProvider.Info(
                $"Property \"{propertyInfo.Name}\" processed with success.");
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
                        dataRow[keyValuePair.Key] = HttpUtility.HtmlEncode(keyValuePair.Value);
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