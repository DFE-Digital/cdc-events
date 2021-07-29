namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Processes incoming data into stored json objects.
    /// </summary>
    public interface IEntityArchiveProcessor
    {
        /// <summary>
        /// Accepts a raw JSON object and stores it in an object store.
        /// </summary>
        /// <param name="entityType">The type name of the entity data being stored.</param>
        /// <param name="runIdentifier">
        /// An identifier for the run, as a <see cref="DateTime" /> value.
        /// </param>
        /// <param name="data">An instance of the json data as a string.</param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task CreateAsync(
            string entityType,
            DateTime runIdentifier,
            string data,
            CancellationToken cancellationToken);
    }
}
