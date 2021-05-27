namespace Dfe.CdcEventApi.Domain.Models
{
    /// <summary>
    /// Represents an individual <c>blob</c> entity.
    /// </summary>
    public class StorageBlob : LoadBase
    {
        /// <summary>
        /// Gets or sets the mime type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        public string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets the stored blob data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Its necessary to store binary data in the table")]
        public byte[] BlobData { get; set; } = System.Array.Empty<byte>();

        /// <summary>
        /// Gets or sets the pernt object type name.
        /// </summary>
        public string ParentObjectType { get; set; }

        /// <summary>
        /// Gets or sets the parent object identifier.
        /// </summary>
        public string ParentObjectId { get; set; }

        /// <summary>
        /// Gets or sets the Run Identifier date time stamp.
        /// </summary>
        public string RunIdentifier { get; set; }
    }
}