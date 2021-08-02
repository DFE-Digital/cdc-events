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
        /// <summary>
        /// Gets the mastered DB connection string.
        /// </summary>
        public string MasteredDbConnectionString
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("MasteredDbConnectionString");

                return toReturn;
            }
        }

        /// <inheritdoc />
        public string RawDbConnectionString
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("RawDbConnectionString");

                return toReturn;
            }
        }
    }
}