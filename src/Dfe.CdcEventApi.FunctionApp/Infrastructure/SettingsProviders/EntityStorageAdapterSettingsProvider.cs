namespace Dfe.CdcEventApi.FunctionApp.Infrastructure.SettingsProviders
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Implements <see cref="IEntityStorageAdapterSettingsProvider" />.
    /// </summary>
    public class EntityStorageAdapterSettingsProvider
        : IEntityStorageAdapterSettingsProvider
    {
        /// <inheritdoc />
        public string RawDbConnectionString
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable(
                    "RawDbConnectionString");

                return toReturn;
            }
        }
    }
}