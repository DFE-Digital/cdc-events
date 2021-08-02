namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions;

    /// <summary>
    /// Represents an individual <c>blob</c> entity.
    /// </summary>
    public class AttachmentResponse : ControlBase
    {
        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        public string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets the recieved blob data object. Due to Azure this is of variable type handled by an <see cref="IBlobConvertor"/>.
        /// </summary>
        public object BlobData { get; set; }

        /// <summary>
        /// Gets or sets the file share name.
        /// </summary>
        public string BlobShare { get; set; }

        /// <summary>
        /// Gets or sets the file mimetype.
        /// </summary>
        public string BlobMimeType { get; set; }

        /// <summary>
        /// Gets or sets the file folder.
        /// </summary>
        public string BlobFolder { get; set; }

        /// <summary>
        /// Gets or sets the file name with extension.
        /// </summary>
        public string BlobFilename { get; set; }

        /// <summary>
        /// Gets or sets the Site id.
        /// </summary>
        public string BlobSiteUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the Site URN.
        /// </summary>
        public int BlobSiteUrn { get; set; }

        /// <summary>
        /// Gets or sets the CDC File location file type.
        /// </summary>
        public int BlobFiletype { get; set; } = 3;

        /// <summary>
        /// Gets or sets the entity unique id.
        /// </summary>
        public string BlobEntityUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the Obained date and time.
        /// </summary>
        public DateTime BlobObtained { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Its a DTO")]
        public string BlobUrl { get; set; }

        /// <summary>
        /// Gets or sets the blob usage Entity.
        /// </summary>
        public string BlobEntity { get; set; }

        /// <summary>
        /// Gets or sets the blob site name.
        /// </summary>
        public string BlobSiteName { get; set; }
    }
}