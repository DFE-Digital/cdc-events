namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An enumeration of available Load states.
    /// </summary>
    [Flags]
    public enum LoadStates
    {
        /// <summary>
        /// No state has yet been set as nothing has happend.
        /// </summary>
        [Description("Nothing")] None = 0,

        /// <summary>
        /// The load has started running.
        /// </summary>
        [Description("Failed")] Loading = 1,

        /// <summary>
        /// The load has failed when initialising.
        /// </summary>
        [Description("Init Failed")] FailedOnInitialisation = 2,

        /// <summary>
        /// The load has failed during entity loading.
        /// </summary>
        [Description("Entities Failed")] FailedOnEntities = 4,

        /// <summary>
        /// The load has failed when processing blobs.
        /// </summary>
        [Description("BLOBS Failed")] FailedOnBlobHandling = 8,

        /// <summary>
        /// The load has succeeded.
        /// </summary>
        [Description("Succeded")] Suceeeded = 32,
    }
}
