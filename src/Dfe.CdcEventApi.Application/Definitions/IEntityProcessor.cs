namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
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
        /// <param name="runIdentifier">
        /// An identifier for the run, as a <see cref="DateTime" /> value.
        /// </param>
        /// <param name="modelsBases">
        /// A collection of instances of type
        /// <typeparamref name="TModelsBase" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task CreateEntitiesAsync<TModelsBase>(
            DateTime runIdentifier,
            IEnumerable<TModelsBase> modelsBases,
            CancellationToken cancellationToken)
            where TModelsBase : ModelsBase;
    }
   
}