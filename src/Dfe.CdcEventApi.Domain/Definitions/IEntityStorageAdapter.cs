namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

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
        /// <param name="dataTable">
        /// The datatable containing the entities.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task StoreEntitiesAsync(
            string dataHandlerIdentifier,
            DataTable dataTable,
            CancellationToken cancellationToken);
    }
}