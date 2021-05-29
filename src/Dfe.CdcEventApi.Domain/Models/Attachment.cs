namespace Dfe.CdcEventApi.Domain.Models
{
    /// <summary>
    /// Represents an attachment entity.
    /// </summary>
    public class Attachment
    {

        /// <summary>
        /// Gets or sets a value for the blob key.
        /// </summary>
        public string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets a mime type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets a value for the parent object type.
        /// </summary>
        public string ParentObjectType { get; set; }

        /// <summary>
        /// Gets or sets a value for the parent object id.
        /// </summary>
        public string ParentObjectId { get; set; }
    }
}
