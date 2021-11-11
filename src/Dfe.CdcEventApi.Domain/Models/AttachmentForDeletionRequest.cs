namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Represents an attachment which is queued for deletion.
    /// </summary>
    public class AttachmentForDeletionRequest : IAttachmentForDeletionRequest
    {
        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        public string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets the entity unique id.
        /// </summary>
        public string EntityUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the EvidenceType.
        /// </summary>
        public string EvidenceType { get; set; }
    }
}
