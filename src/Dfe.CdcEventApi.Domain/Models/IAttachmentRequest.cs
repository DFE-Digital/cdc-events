namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Defines the properties of the API attachment request.
    /// </summary>
    public interface IAttachmentRequest
    {
        /// <summary>
        /// Gets or sets the blob key.
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Gets or sets the file share name.
        /// </summary>
        string ShareName { get; set; }

        /// <summary>
        /// Gets or sets the file mimetype.
        /// </summary>
        string MimeType { get; set; }

        /// <summary>
        /// Gets or sets the file folder.
        /// </summary>
        string FolderName { get; set; }

        /// <summary>
        /// Gets or sets the file name with extension.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the Site id.
        /// </summary>
        string SiteUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the Site URN.
        /// </summary>
        int SiteURN { get; set; }

        /// <summary>
        /// Gets or sets the entity unique id.
        /// </summary>
        string EntityUniqueId { get; set; }

        /// <summary>
        /// Gets or sets the Obained date and time.
        /// </summary>
        DateTime Obtained { get; set; }

        /// <summary>
        /// Gets or sets the Url.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:URI-like properties should not be strings", Justification = "Required for internal processing of a DTO")]
        string Url { get; set; }

        /// <summary>
        /// Gets or sets the blob usage Entity.
        /// </summary>
        string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the blob site name.
        /// </summary>
        string SiteName { get; set; }

        /// <summary>
        /// Gets or sets a value for the Extension.
        /// </summary>
        string Extension { get; set; }

        /// <summary>
        /// Gets or sets the CDC File location file type.
        /// Should default to 3 for evidence attachments.
        /// Values set should be one of the CDC File Location File Types.
        /// e.g. 1,2 or [3].
        /// </summary>
        int FileType { get; set; }
    }
}