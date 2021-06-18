namespace Dfe.CdcEventApi.Domain.Models
{
    /// <summary>
    /// Represents an individual <c>blob</c> entity.
    /// </summary>
    public class Blob : LoadBase
    {
        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        public string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }
    }
}