namespace Dfe.CdcEventApi.Domain.Definitions.SettingsProviders
{
    /// <summary>
    /// Describes the operations of the <see cref="IEntityStorageAdapter" />
    /// settings provider.
    /// </summary>
    public interface IEntityStorageAdapterSettingsProvider
    {
        /// <summary>
        /// Gets the connection string to the "raw" database.
        /// </summary>
        string RawDbConnectionString
        {
            get;
        }
    }
}