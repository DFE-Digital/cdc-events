namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Stores the entity data in the archive storage account.
    /// </summary>
    public interface IEntityArchiveAdapter
    {
        /// <summary>
        /// Stores raw entities in the underlying storage account.
        /// </summary>
        /// <param name="itemName">
        /// The entity type of the data..
        /// </param>
        /// <param name="itemData">The JSON data as a string.</param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task StoreAsync(
            string itemName,
            string itemData,
            CancellationToken cancellationToken);
    }
}