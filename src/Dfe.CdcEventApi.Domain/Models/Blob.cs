namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Represents an individual <c>blob</c> entity.
    /// </summary>
    public class Blob : LoadBase
    {
        /// <summary>
        /// Gets or sets the Site id.
        /// </summary>
        public string SiteUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        public string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Its a DTO")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Obained date and time.
        /// </summary>
        public DateTime Obtained { get; set; }
    }
}