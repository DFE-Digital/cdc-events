namespace Dfe.CdcEventApi.Application
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Implements <see cref="IEntityProcessor" />.
    /// </summary>
    public class EntityProcessor : IEntityProcessor
    {
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityProcessor" />
        /// class.
        /// </summary>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public EntityProcessor(ILoggerProvider loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

        /// <inheritdoc />
        public async Task ProcessEntitiesAsync<TModelsBase>(
            IEnumerable<TModelsBase> modelsBases)
            where TModelsBase : ModelsBase
        {
            // 1) Prepare the main DataTable to be passed to the data-layer.
            DataTable dataTable = this.ConvertToDataTable(modelsBases);

            // TODO:
            // 2) Figure out which embedded TSQL script to invoke from the
            //    meta-data of TModels base.
            // 3) Invoke the data-layer with the script and the DataTable.
            // 4) Recursively check the model meta-data for embedded
            //    collections.
        }

        private DataTable ConvertToDataTable(
            IEnumerable<ModelsBase> modelsBases)
        {
            DataTable toReturn = null;

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

            toReturn = new DataTable();

            DataColumn[] dataColumns = distinctColumnNames
                .Select(x => new DataColumn(x))
                .ToArray();

            toReturn.Columns.AddRange(dataColumns);

            this.loggerProvider.Debug(
                $"{dataColumns.Length} {nameof(DataColumn)}s added to " +
                $"{nameof(toReturn)}.");

            DataRow dataRow = null;
            foreach (IDictionary<string, JToken> data in datas)
            {
                dataRow = toReturn.NewRow();

                foreach (KeyValuePair<string, JToken> keyValuePair in data)
                {
                    dataRow[keyValuePair.Key] = keyValuePair.Value;
                }

                toReturn.Rows.Add(dataRow);
            }

            return toReturn;
        }
    }
}