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
        string APIKey { get; }

        /// <summary>
        /// Gets the Success template Id to use.
        /// </summary>
        string SuccessTemplateId { get; }

        /// <summary>
        /// Gets the Failure template Id to use.
        /// </summary>
        string FailureAddresses { get; }

        /// <summary>
        /// Gets the Success email addresses.
        /// </summary>
        string SucesssAddresses { get; }
    }
}