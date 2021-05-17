namespace Dfe.CdcEventApi.Application.Models
{

    using System;

    /// <summary>
    /// An enumeration of available Load states.
    /// </summary>
    [Flags]
    public enum LoadStates
    {
        /// <summary>
        /// The load is running.
        /// </summary>
        Loading = 0,

        /// <summary>
        /// The load has failed initialising.
        /// </summary>
        FailedOnInitialisation = 1,


        /// <summary>
        /// The load has failed entity loading.
        /// </summary>
        FailedOnEntities = 2,

        /// <summary>
        /// The load has failed processing blobs.
        /// </summary>
        FailedOnBlobHandling = 4,


        /// <summary>
        /// The load has failed entity loading.
        /// </summary>
        FailedOnControl = 8,

        /// <summary>
        /// The load has succeeded.
        /// </summary>
        Suceeeded = 16,
    }

    
}
