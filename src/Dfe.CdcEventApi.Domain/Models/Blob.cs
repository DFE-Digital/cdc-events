namespace Dfe.CdcEventApi.Domain.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an individual <c>blob</c> entity.
    /// </summary>
    public class Blob : LoadBase
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
        /// Gets or sets the blob data.
        /// </summary>
        public dynamic BlobData { get; set; }

        /// <summary>
        /// Gets the blob data as a string.
        /// </summary>
        public string BlobContent
        {
            get
            {
                string data = JsonConvert.SerializeObject(this.BlobData);
                return data;
            }
        }

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