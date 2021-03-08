namespace Dfe.CdcEventApi.Application.Definitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Models;

    /// <summary>
    /// Describes the operations of the entity processor.
    /// </summary>
    public interface IEntityProcessor
    {
        /// <summary>
        /// Accepts a collection of instances of type
        /// <typeparamref name="TModelsBase" />, and processes them.
        /// </summary>
        /// <typeparam name="TModelsBase">
        /// A type deriving from <see cref="ModelsBase" />.
        /// </typeparam>
        /// <param name="modelsBases">
        /// A collection of instances of type
        /// <typeparamref name="TModelsBase" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task ProcessEntitiesAsync<TModelsBase>(
            IEnumerable<TModelsBase> modelsBases)
            where TModelsBase : ModelsBase;
    }
}