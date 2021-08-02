namespace Dfe.CdcEventApi.Domain.Models
{
    using System.ComponentModel;

    /// <summary>
    /// An enumeration of possible control states. These values are depended upon by the Events-Generator-Api.
    /// See notes below.
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
        /// The orchestrator task has reported the data as delivered.
        /// This value is depended upon by SQL Code in the downstream ETL Process in the CDC-Raw-Data project and Azure Data Factory processes.
        /// IMPORTANT NOTE:
        /// Further step values in the etl.Control table Status column are;
        /// Extracted = 4, transformed = 5, Finisheded = 6
        /// which are handled by the ETL extract and Transform process in other system components.
        /// </summary>
        [Description("Delivered")]
        Delivered = 3,

        /// <summary>
        /// This is handled by downstream ETL processing, and indicates that the data was onward extracted.
        /// </summary>
        [Description("Extracted")]
        Extracted = 4,

        /// <summary>
        /// This is handled by downstream ETL processing, and indicates that the data was onward trasnformed.
        /// </summary>
        [Description("Transformed")]
        Transformed = 5,

        /// <summary>
        /// This is handled by downstream ETL processing, and indicates that the data was onward delivered and a cleanup was pereformed.
        /// </summary>
        [Description("Finished")]
        Finished = 6,
    }
}
