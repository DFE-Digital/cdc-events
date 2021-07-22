namespace Dfe.CdcEventApi.Domain.Models
{
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions;

    /// <summary>
    /// Represents the Load reporting entity.
    /// </summary>
    public class LoadNotification : LoadBase
    {
        /// <summary>
        /// Gets or sets the entity id.
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore this entity instance.
        /// </summary>
        public bool Active
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity status.
        /// </summary>
        public ControlState Status
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity group membership.
        /// </summary>
        public string Group
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the entity email address.
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include row statistics.
        /// </summary>
        public bool IncludeRowStats
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to include run information.
        /// </summary>
        public bool IncludeRunInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to  include referential integrity checks.
        /// </summary>
        public bool IncludeRIChecks
        {
            get;
            set;
        }
    }
}
