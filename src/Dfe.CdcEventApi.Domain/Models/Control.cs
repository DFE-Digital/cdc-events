namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// Represents the LoadControl entity.
    /// </summary>
    public class Control : ControlBase
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
        /// Gets or sets the Since Date Time.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "This is a DTO")]
        public DateTime? Since_DateTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Finish time.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "This is a DTO")]
        public DateTime? Finished_DateTime
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
        public ControlState Status
        {
            get;
            set;
        }
    }
}
