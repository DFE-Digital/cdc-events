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
            short status,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the notifications list for load result notifications.
        /// </summary>
        /// <param name="status">The status of the load to filter notifications.</param>
        /// <param name="cancellationToken">The asyncrhonous cancellation token.</param>
        /// <returns>A collection of <see cref="LoadNotification"/> entities.</returns>
        Task<IEnumerable<LoadNotification>> GetLoadNotifications(
            short status,
            CancellationToken cancellationToken);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <param name="cancellationToken">....</param>
        /// <returns>...</returns>
        Task<Load> GetLoadAsync(
            DateTime runIdentifier,
            CancellationToken cancellationToken);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="item">..</param>
        /// <param name="cancellationToken">....</param>
        /// <returns>...</returns>
        Task UpdateLoadAsync(
            Load item,
            CancellationToken cancellationToken);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="status">..</param>
        /// <param name="cancellationToken">...</param>
        /// <returns>....</returns>
        Task<LoadNotificationTemplate> GetLoadTemplateForStatus(
            short status,
            CancellationToken cancellationToken);
    }
}
