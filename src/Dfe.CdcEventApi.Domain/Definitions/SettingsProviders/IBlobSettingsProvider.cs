namespace Dfe.CdcEventApi.Domain.Definitions.SettingsProviders
{
    /// <summary>
    /// Describes Blob handling adjustments.
    /// </summary>
    public interface IBlobSettingsProvider
    {
        /// <summary>
        /// Gets the file share connection stirng.
        /// </summary>
        string BlobStorageConnectionString { get; }

        /// <summary>
        /// Gets the file share account name value the is used for generating (SAS) Secured Access Signature query parameters and prefixing the Blob's Url itself.
        /// </summary>
        string BlobStorageAccountName { get; }

        /// <summary>
        /// Gets the account Key value for generating (SAS) Secured Access Signature query parameters.
        /// </summary>
        string BlobStorageAccountKey { get; }
    }
}