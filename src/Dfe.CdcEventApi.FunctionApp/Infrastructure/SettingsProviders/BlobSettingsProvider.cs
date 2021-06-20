﻿namespace Dfe.CdcEventApi.FunctionApp.Infrastructure.SettingsProviders
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Implements <see cref="IBlobSettingsProvider"/>.
    /// </summary>
    public class BlobSettingsProvider : IBlobSettingsProvider
    {
        /// <inheritdoc />
        public string AttachmentUrlPrefix
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                        "AttachmentUrlPrefix");

                return toReturn;
            }
        }
    }
}