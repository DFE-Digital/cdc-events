namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An enumeration of possible control states. These values are depended upon by the Events-Generator-Api.
    /// </summary>
    public enum ControlState : int
    {
        /// <summary>
        /// The task has started running.
        /// </summary>
        [Description("Starting")]
        Start = 0,

        /// <summary>
        /// The load is downloading raw data entities.
        /// </summary>
        [Description("Processing Entities")]
        Entities = 1,

        /// <summary>
        /// The task is obtaining the attachments
        /// </summary>
        [Description("Processing Attachments")]
        Attachments = 2,

        /// <summary>
        /// The task is reporting results. This value is depended upon by SQL Code in teh ETL Process.
        /// </summary>
        [Description("Completed")]
        Reporting = 3,
    }
}
