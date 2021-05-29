namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using Dfe.CdcEventApi.Application.Definitions;

    /// <summary>
    /// Represents the LoadControl entity.
    /// </summary>
    public class Load : LoadBase
    {
        /// <summary>
        /// Gets or sets the Load Date Time.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "This is a DTO")]
        public DateTime Load_DateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Finish time.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "This is a DTO")]
        public DateTime Finish_DateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the load count.
        /// </summary>
        public int Count
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the load status.
        /// </summary>
        public LoadStates Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the report title describing the load run.
        /// </summary>
        public string ReportTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the report body describing the laod run.
        /// </summary>
        public string ReportBody
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a separated list of email addresses to send report to.
        /// </summary>
        public string ReportTo
        {
            get;
            set;
        }
    }
}
