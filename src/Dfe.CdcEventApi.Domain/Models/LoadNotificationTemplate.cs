namespace Dfe.CdcEventApi.Domain.Models
{
    /// <summary>
    /// Represetns a load run report template.
    /// </summary>
    public class LoadNotificationTemplate
    {
        /// <summary>
        /// Gets or sets .
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets .
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether .
        /// </summary>
        public bool IncludeRowStats { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether .
        /// </summary>
        public bool IncludeRIChecks { get; set; }
    }
}
