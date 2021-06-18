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
        /// Gets or sets a value for the folder.
        /// </summary>
        public string Folder { get; set; }

        /// <summary>
        /// Gets or sets a value for the Extension.
        /// </summary>
        public string Extension { get; set; }
    }
}
