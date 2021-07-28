namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Describes the operations of the blob processor.
    /// </summary>
    public interface IAttachmentProcessor
    {
        /// <summary>
        /// Accepts a collection of instances of type<see cref="AttachmentResponse"/>, and processes them.
        /// </summary>
        /// <param name="runIdentifier">
        /// An identifier for the run, as a <see cref="DateTime" /> value.
        /// </param>
        /// <param name="models">
        /// A collection of instances of type<see cref="AttachmentResponse"/>.
        /// </param>
        /// <param name="blobStorageConnectionString">The file store connection string.</param>
        /// <param name="blobStorageAccountName">The file storage account name.</param>
        /// <param name="blobStorageAccountKey">The file storage Shared Access Signature (SAS) key.</param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        Task PostAsync(
            DateTime runIdentifier,
            IEnumerable<AttachmentResponse> models,
            string blobStorageConnectionString,
            string blobStorageAccountName,
            string blobStorageAccountKey,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the currently required collection of <see cref="AttachmentRequest"/>.
        /// </summary>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>A collection of <see cref="AttachmentRequest"/>.</returns>
        Task<IEnumerable<AttachmentRequest>> GetAsync(CancellationToken cancellationToken);
    }
}