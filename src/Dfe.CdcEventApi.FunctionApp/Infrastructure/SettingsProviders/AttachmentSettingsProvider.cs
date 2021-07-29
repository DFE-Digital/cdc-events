namespace Dfe.CdcEventApi.FunctionApp.Infrastructure.SettingsProviders
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Implements <see cref="IAttachmentSettingsProvider"/>.
    /// </summary>
    public class AttachmentSettingsProvider : IAttachmentSettingsProvider
    {
        /// <inheritdoc/>
        public string AttachmentStorageAccountName
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("AttachmentStorageAccountName");

                return toReturn;
            }
        }

        /// <inheritdoc/>
        public string AttachmentStorageAccountKey
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("AttachmentStorageAccountKey");

                return toReturn;
            }
        }

        /// <inheritdoc/>
        public string AttachmentStorageConnectionString
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("AttachmentStorageConnectionString");

                return toReturn;
            }
        }

        /// <inheritdoc/>
        public string ArchiveContainerName
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("ArchiveContainerName");

                return toReturn;
            }
        }
    }
}