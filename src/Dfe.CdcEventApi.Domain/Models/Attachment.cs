namespace Dfe.CdcEventApi.Domain.Models
{
    /// <summary>
    /// Represents an attachment entity.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Gets or sets a value for the site name.
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// Gets or sets a value for the site id.
        /// </summary>
        public string SiteId { get; set; }

        /// <summary>
        /// Gets or sets a value for the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value for the blob sub folder name.
        /// </summary>
        public string BlobSubFolderNAme { get; set; }

        /// <summary>
        /// Gets or sets a value for the survey identifier.
        /// </summary>
        public string SurveyId { get; set; }

        /// <summary>
        /// Gets or sets a value for the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value for the blob mime type.
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// Gets or sets a value for the blob id.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets a value for the blob key.
        /// </summary>
        public string BlobKey { get; set; }

        /// <summary>
        /// Gets or sets a value for the parent object type.
        /// </summary>
        public string ParentObjectType { get; set; }

        /// <summary>
        /// Gets or sets a value for the parent object id.
        /// </summary>
        public string ParentObjectId { get; set; }

        /// <summary>
        /// Gets or sets a value for the actual blob data.
        /// </summary>
        public byte[] BlobData { get; set; }

        /// <summary>
        /// Gets or sets a value for the blob url.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a value for the entity status.
        /// </summary>
        public string EntityStatus { get; set; }

        /// <summary>
        /// Gets or sets a value for the load date and time.
        /// </summary>
        public string Load_DateTime { get; set; }

        /// <summary>
        /// Gets or sets a value for the upload date time.
        /// </summary>
        public string Upload_DateTime { get; set; }
    }
}
