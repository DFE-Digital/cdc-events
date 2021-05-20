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
        /// Gets the <see cref="LoadNotificationTemplate"/> for the specified status value.
        /// </summary>
        /// <param name="status">
        /// The status value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="LoadNotificationTemplate"/>.
        /// </returns>
        Task<LoadNotificationTemplate> GetLoadTemplateForStatus(
            short status,
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
    }
}
