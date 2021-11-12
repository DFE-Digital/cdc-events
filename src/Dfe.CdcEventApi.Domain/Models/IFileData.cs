namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Defines the properties of the FileData.
    /// </summary>
    public interface IFileData
    {
        /// <summary>
        /// Gets or sets the supplier key id.
        /// </summary>
        string SupplierKeyID { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file url.
        /// </summary>
        string FileURL { get; set; }
    }
}
