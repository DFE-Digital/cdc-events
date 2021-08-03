namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Represents the API response for an evidence attachment file.
    /// </summary>
    public class AttachmentResponse : IAttachmentResponse
    {
        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the file share name.
        /// </summary>
        public string ShareName { get; set; }

        /// <summary>
        /// Gets or sets the file mimetype.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the file folder.
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the file name with extension.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Site id.
        /// </summary>
        public string SiteUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the Site URN.
        /// </summary>
        public int SiteURN { get; set; }

        /// <summary>
        /// Gets or sets the entity unique id.
        /// </summary>
        public string EntityUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the Obained date and time.
        /// </summary>
        public DateTime Obtained { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the blob usage Entity.
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the blob site name.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets a value for the Extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the CDC File location file type.
        /// </summary>
        public int FileType { get; set; } = 3;

        /// <summary>
        /// Gets or sets the attachment data.
        /// </summary>
        public object Data { get; set; }
    }
}