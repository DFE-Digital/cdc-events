﻿namespace Dfe.CdcEventApi.Domain.Models
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// An enumeration of possible Load states.
    /// </summary>
    [Flags]
    public enum LoadStates : int
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

        /// <summary>
        /// The transform has finished.
        /// </summary>
        [Description("Finished")]
        Finished = 64,
    }
}
