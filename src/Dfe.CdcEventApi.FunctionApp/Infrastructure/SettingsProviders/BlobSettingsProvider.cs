namespace Dfe.CdcEventApi.FunctionApp.Infrastructure.SettingsProviders
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Implements <see cref="IBlobSettingsProvider"/>.
    /// </summary>
    public class BlobSettingsProvider : IBlobSettingsProvider
    {
        /// <inheritdoc/>
        public string BlobStorageAccountName
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                           "BlobStorageAccountName");

                return toReturn;
            }
        }

        /// <inheritdoc/>
        public string BlobStorageAccountKey
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                       "BlobStorageAccountKey");

                return toReturn;
            }
        }

        public string BlobStorageConnectionString
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                          "BlobStorageConnectionString");

                return toReturn;
            }
        }
    }