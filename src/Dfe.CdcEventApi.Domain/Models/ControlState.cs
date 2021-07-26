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
        /// No state has yet been set as nothing has happend.
        /// </summary>
        [Description("Nothing")]
        None = 0,

        /// <summary>
        /// The task has started running.
        /// </summary>
        [Description("Starting")]
        Start = 1,

        /// <summary>
        /// The load is downloading raw data entities.
        /// </summary>
        [Description("Processing Entities")]
        Entities = 2,

        /// <summary>
        /// The task is extracting the loaded raw data
        /// </summary>
        [Description("Extracting")]
        Extracting = 3,

        /// <summary>
        /// The task is obtaining the attachments
        /// </summary>
        [Description("Processing Attachments")]
        Attachments = 4,

        /// <summary>
        /// The transform has finished.
        /// </summary>
        [Description("Transforming")]
        Transforming = 5,

        /// <summary>
        /// The task is reporting results. This value is depended upon by SQL Code in teh ETL Process.
        /// </summary>
        [Description("Reporting results")]
        Reporting = 100,
    }
}
