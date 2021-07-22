namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An enumeration of possible control states.
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
        /// The load is logging in.
        /// </summary>
        [Description("Source OAUTH login")]
        Login = 2,

        /// <summary>
        /// The load is downloading raw data entities.
        /// </summary>
        [Description("Processing Entities")]
        Entities = 3,

        /// <summary>
        /// The task is extracting the loaded raw data
        /// </summary>
        [Description("Extracting")]
        Extracting = 4,

        /// <summary>
        /// The task is obtaining the attachments
        /// </summary>
        [Description("Attachments")]
        Attachments = 5,

        /// <summary>
        /// The transform has finished.
        /// </summary>
        [Description("Finished")]
        Transforming = 6,

        /// <summary>
        /// The task is reporting results
        /// </summary>
        [Description("Reporting results")]
        Reporting = 100,
    }
}
