namespace Dfe.CdcEventApi.Domain.Definitions.SettingsProviders
{
    /// <summary>
    /// Describes the Notify settings.
    /// </summary>
    public interface INotifySettingsProvider
    {
        /// <summary>
        /// Gets the API Key to use.
        /// </summary>
        string NotifyApiKey { get; }

        /// <summary>
        /// Gets the Success template Id to use.
        /// </summary>
        string NotifySuccessTemplateId { get; }

        /// <summary>
        /// Gets the Failure template Id to use.
        /// </summary>
        string NotifyFailureTemplateId { get; }

        /// <summary>
        /// Gets the Failure template Id to use.
        /// </summary>
        string NotifyFailureAddresses { get; }

        /// <summary>
        /// Gets the Success email addresses.
        /// </summary>
        string NotifySuccesssAddresses { get; }

        /// <summary>
        /// Gets the environment name.
        /// </summary>
        string NotifyEnvironmentName { get; }
    }
}