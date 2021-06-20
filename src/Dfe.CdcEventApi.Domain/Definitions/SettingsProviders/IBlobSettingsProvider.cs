namespace Dfe.CdcEventApi.Domain.Definitions.SettingsProviders
{
    /// <summary>
    /// Describes Blob handling adjustments.
    /// </summary>
    public interface IBlobSettingsProvider
    {
        /// <summary>
        /// Gets the attachment Url prefix to apply to the handling of all incoming blob items.
        /// </summary>
#pragma warning disable CA1056 // URI-like properties should not be strings
        string AttachmentUrlPrefix { get; }
#pragma warning restore CA1056 // URI-like properties should not be strings
    }
}