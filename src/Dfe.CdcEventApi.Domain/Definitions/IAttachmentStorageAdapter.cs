namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Describes the operations of the entity storage adapter.
    /// </summary>
    public interface IAttachmentStorageAdapter
    {
        /// <summary>
        /// Gets the Attachment process instruction records for the current load.
        /// </summary>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="AttachmentRequest"/> of the run.
        /// </returns>
        Task<IEnumerable<AttachmentRequest>> GetAsync();

        /// <summary>
        /// Creats new <see cref="AttachmentResponse"/> records.
        /// </summary>
        /// <param name="runIdentifier">The load run identifier.</param>
        /// <param name="blobStorageConnectionString">The file share connection string.</param>
        /// <param name="blobStorageAccountName">The file storage account name.</param>
        /// <param name="blobStorageAccountKey">The file storage Shared Access Signature (SAS) key.</param>
        /// <param name="attachments">The collection of <see cref="AttachmentResponse"/> entities.</param>
        /// <returns>An <see cref="Task"/>.</returns>
        Task CreateAsync(
            DateTime runIdentifier,
            string blobStorageConnectionString,
            string blobStorageAccountName,
            string blobStorageAccountKey,
            IEnumerable<AttachmentResponse> attachments);
    }
}