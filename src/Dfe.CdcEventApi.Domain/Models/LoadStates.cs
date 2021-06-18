namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An enumeration of possible Load states.
    /// </summary>
    [Flags]
#pragma warning disable CA1028 // Enum Storage should be Int32
    public enum LoadStates : short
#pragma warning restore CA1028 // Enum Storage should be Int32
    {
        /// <summary>
        /// No state has yet been set as nothing has happend.
        /// </summary>
        [Description("Nothing")]
        None = 0,

        /// <summary>
        /// The load has started running.
        /// </summary>
        [Description("Start Failed")]
        Initialising = 1,

        /// <summary>
        /// The load is retrieving secrets.
        /// </summary>
        [Description("Preparation failed")]
        Preparing = 2,

        /// <summary>
        /// The load is logging in.
        /// </summary>
        [Description("Session failed")]
        Login = 4,

        /// <summary>
        /// The load is downloading entities.
        /// </summary>
        [Description("Entities Failed")]
        Entities = 8,

        /// <summary>
        /// The load has finished.
        /// </summary>
        [Description("Succeded")]
        Suceeeded = 32,
    }
}
