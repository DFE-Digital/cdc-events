namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Describes the operations of the entity storage adapter.
    /// </summary>
    public interface ILoadStorageAdapter
    {
        /// <summary>
        /// Starts the load by creating and returning a new <see cref="Load"/> model.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an instance of <see cref="Load"/> of the run start.
        /// </returns>
        Task<IEnumerable<Load>> CreateLoadAsync(DateTime runIdentifier);

        /// <summary>
        /// Updates the status of a <see cref="Load"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The identifier of the <see cref="Load"/>.
        /// </param>
        /// <param name="status">
        /// The valof the new status.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        Task UpdateLoadStatusAsync(DateTime runIdentifier, short status);

        /// <summary>
        /// Gets the <see cref="Load"/> for the specified date and time.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="Load"/> of the run.
        /// </returns>
        Task<Load> GetLoadAsync(DateTime runIdentifier);

        /// <summary>
        /// Gets the report template for the specified status of any load.
        /// </summary>
        /// <param name="status">
        /// The status value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="LoadNotificationTemplate"/>.
        /// </returns>
        Task<IEnumerable<LoadNotification>> GetLoadNotificationsForStatus(short status);

        /// <summary>
        /// Updates a <see cref="Load"/> of the specified date and time.
        /// </summary>
        /// <param name="item">
        /// The new version of the <see cref="Load"/> item.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        Task UpdateLoadAsync(Load item);

        /// <summary>
        /// Gets the report template for the specified status of any load.
        /// </summary>
        /// <param name="status">
        /// The status value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="LoadNotificationTemplate"/>.
        /// </returns>
        Task<LoadNotificationTemplate> GetLoadTemplateForStatus(short status);

        /// <summary>
        /// Gets the Attechment process instruction records for the current load.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="Attachment"/> of the run.
        /// </returns>
        Task<IEnumerable<Attachment>> GetAttachments(DateTime runIdentifier);

        /// <summary>
        /// Creats new <see cref="Blob"/> records.
        /// </summary>
        /// <param name="runIdentifier">The load run identifier.</param>
        /// <param name="blobs">The collection of <see cref="Blob"/> entities.</param>
        /// <returns>An <see cref="Task"/>.</returns>
        Task CreateBlobsAsync(DateTime runIdentifier, IEnumerable<Blob> blobs);
    }
}