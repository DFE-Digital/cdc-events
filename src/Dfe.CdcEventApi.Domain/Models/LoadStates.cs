namespace Dfe.CdcEventApi.Domain.Models
{
    using System;

    /// <summary>
    /// An enumeration of available Load states.
    /// </summary>
    [Flags]
    public enum LoadStates
    {
        /// <summary>
        /// No state has yet been set as nothing has happend.
        /// </summary>
        None = 0,

        /// <summary>
        /// The load has started running.
        /// </summary>
        Loading = 1,

        /// <summary>
        /// The load has failed when initialising.
        /// </summary>
        FailedOnInitialisation = 2,

        /// <summary>
        /// The load has failed during entity loading.
        /// </summary>
        FailedOnEntities = 4,

        /// <summary>
        /// The load has failed when processing blobs.
        /// </summary>
        FailedOnBlobHandling = 8,

        /// <summary>
        /// The load has succeeded.
        /// </summary>
        Suceeeded = 32,
    }
}
