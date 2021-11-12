namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Defines the properties of the FileData.
    /// </summary>
    public class FileData : IFileData
    {
        /// <summary>
        /// Gets or sets the supplier key id.
        /// </summary>
        public string SupplierKeyID { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file url.
        /// </summary>
        public string FileURL { get; set; }
    }
}
