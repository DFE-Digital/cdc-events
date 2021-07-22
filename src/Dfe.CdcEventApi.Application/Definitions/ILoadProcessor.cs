namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Describes the operations of the load processor.
    /// </summary>
    public interface ILoadProcessor
    {
        /// <summary>
        /// Starts a new load event by creating and reurning a new <see cref="Load"/> record.
        /// </summary>
        /// <param name="runIdentifier">The load start date and time.</param>
        /// <param name="cancellationToken">The asyncrhonous cancellation token.</param>
        /// <returns>The created <see cref="Load"/> record with its default values.</returns>
        Task<IEnumerable<Load>> CreateLoadAsync(
            DateTime runIdentifier,
            CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing <see cref="Load"/> record.
        /// </summary>
        /// <param name="runIdentifier">The load start date and time.</param>
        /// <param name="status">The finish status of the load run.</param>
        /// <param name="cancellationToken">The asyncrhonous cancellation token.</param>
        /// <returns>The asynchronous task.</returns>
        Task UpdateLoadStatusAsync(
            DateTime runIdentifier,
            int status,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets an instance of <see cref="Load"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The instance identifier value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="Load"/> instance.
        /// </returns>
        Task<Load> GetLoadAsync(
            DateTime runIdentifier,
            CancellationToken cancellationToken);

        /// <summary>
        /// Uploads a complete instance of <see cref="Load"/>.
        /// </summary>
        /// <param name="item">
        /// The changed <see cref="Load"/> instance.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        Task UpdateLoadAsync(
            Load item,
            CancellationToken cancellationToken);

        /// <summary>
        /// Get all <see cref="Attachment"/> instances deriving from the specfied <see cref="Load "/> date and time.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the collection of <see cref="Attachment"/>.
        /// </returns>
        Task<IEnumerable<Attachment>> GetAttachments(DateTime runIdentifier, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the loaded row count for the run.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping an <see cref="int"/>.
        /// </returns>
        Task<int> GetLoadCountAsync(DateTime runIdentifier, CancellationToken cancellationToken);

        /// <summary>
        /// Performs end of run processing to extract the data into the ETL model.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        Task ExecuteExtract(DateTime runIdentifier, CancellationToken cancellationToken);

        /// <summary>
        /// Performs the transformation process from etl model into condition model.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        Task ExecuteTransform(DateTime runIdentifier, CancellationToken cancellationToken);
    }
}
