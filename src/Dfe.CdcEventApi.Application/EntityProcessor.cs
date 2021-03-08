namespace Dfe.CdcEventApi.Application
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Models;

    /// <summary>
    /// Implements <see cref="IEntityProcessor" />.
    /// </summary>
    public class EntityProcessor : IEntityProcessor
    {
        /// <inheritdoc />
        public Task ProcessEntitiesAsync<TModelsBase>(
            IEnumerable<TModelsBase> modelsBases)
            where TModelsBase : ModelsBase
        {
            // TODO -
            throw new System.NotImplementedException();
        }
    }
}