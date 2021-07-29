namespace Dfe.CdcEventApi.Domain.Definitions.SettingsProviders
{
    /// <summary>
    /// Describes Blob handling adjustments.
    /// </summary>
    public interface IAttachmentSettingsProvider
    {
        /// <summary>
        /// The name of the conainer for entity archival.
        /// </summary>
        string ArchiveContainerName { get; }

        /// <summary>
        /// Gets the file share connection stirng.
        /// </summary>
        string AttachmentStorageConnectionString { get; }

        /// <summary>
        /// Gets the file share account name value the is used for generating (SAS) Secured Access Signature query parameters and prefixing the Blob's Url itself.
        /// </summary>
        string AttachmentStorageAccountName { get; }

        /// <summary>
        /// Gets the account Key value for generating (SAS) Secured Access Signature query parameters.
        /// </summary>
        string AttachmentStorageAccountKey { get; }
    }
}