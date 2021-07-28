namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Describes the operations of the control processor.
    /// </summary>
    public interface IControlProcessor
    {
        /// <summary>
        /// Starts a new control event by creating and returning a new <see cref="Control"/> record.
        /// </summary>
        /// <param name="runIdentifier">The load start date and time.</param>
        /// <param name="cancellationToken">The asyncrhonous cancellation token.</param>
        /// <returns>The created <see cref="Control"/> record with its default values.</returns>
        Task<IEnumerable<Control>> CreateAsync(
            DateTime runIdentifier,
            CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing <see cref="Control"/> record.
        /// </summary>
        /// <param name="runIdentifier">The load start date and time.</param>
        /// <param name="status">The finish status of the load run.</param>
        /// <param name="cancellationToken">The asyncrhonous cancellation token.</param>
        /// <returns>The asynchronous task.</returns>
        Task UpdateStatusAsync(
            DateTime runIdentifier,
            int status,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets an instance of <see cref="Control"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The instance identifier value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="Control"/> instance.
        /// </returns>
        Task<Control> GetAsync(
            DateTime runIdentifier,
            CancellationToken cancellationToken);

        /// <summary>
        /// Uploads a complete instance of <see cref="Control"/>.
        /// </summary>
        /// <param name="item">
        /// The changed <see cref="Control"/> instance.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        Task UpdateAsync(
            Control item,
            CancellationToken cancellationToken);

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
        Task<int> GetCountAsync(DateTime runIdentifier, CancellationToken cancellationToken);
    }
}
