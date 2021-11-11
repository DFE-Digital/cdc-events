namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Defines the properties of the AttachmentForDeletionRequest.
    /// </summary>
    public interface IAttachmentForDeletionRequest
    {
        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets the entity unique id.
        /// </summary>
        string EntityUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the EvidenceType.
        /// </summary>
        string EvidenceType { get; set; }
    }
}
