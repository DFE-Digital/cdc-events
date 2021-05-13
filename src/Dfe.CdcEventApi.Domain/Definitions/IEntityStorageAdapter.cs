namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Describes the operations of the entity storage adapter.
    /// </summary>
    public interface IEntityStorageAdapter
    {
        /// <summary>
        /// Stores entities in the underlying storage.
        /// </summary>
        /// <param name="dataHandlerIdentifier">
        /// The data handler identifier.
        /// </param>
        /// <param name="runIdentifier">
        /// An identifier for the run, as a <see cref="DateTime" /> value.
        /// </param>
        /// <param name="xDocument">
        /// The <see cref="XDocument" /> containing the entities.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task StoreEntitiesAsync(
            string dataHandlerIdentifier,
            DateTime runIdentifier,
            XDocument xDocument,
            CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves entities in the underlying storage.
        /// </summary>
        /// <param name="dataHandlerIdentifier">
        /// The data handler identifier.
        /// </param>
        /// <param name="runIdentifier">
        /// An identifier for the run, as a <see cref="DateTime" /> value.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task<IEnumerable<dynamic>> RetrieveEntitiesAsync(
            string dataHandlerIdentifier,
            DateTime runIdentifier,
            CancellationToken cancellationToken);
    }
}