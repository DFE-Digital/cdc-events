namespace Dfe.CdcEventApi.FunctionApp.Infrastructure.SettingsProviders
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Provide the notification system configuration.
    /// </summary>
    public class NotifySettingsProvider : INotifySettingsProvider
    {
        /// <summary>
        /// Gets the API key for the CDC notifications account.
        /// </summary>
        public string NotifyApiKey
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("NotifyApiKey");

                return toReturn;
            }
        }

        /// <summary>
        /// Gets the Success Template Identifier.
        /// </summary>
        public string NotifySuccessTemplateId
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("NotifySuccessTemplateId");

                return toReturn;
            }
        }

        /// <summary>
        /// Gets the Failure Template Identifier.
        /// </summary>
        public string NotifyFailureTemplateId
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("NotifyFailureTemplateId");

                return toReturn;
            }
        }

        /// <summary>
        /// Gets the Success email addresses.
        /// </summary>
        public string NotifyFailureAddresses
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("NotifyFailureAddresses");

                return toReturn;
            }
        }

        /// <summary>
        /// Gets the Success email addresses.
        /// </summary>
        public string NotifySuccesssAddresses
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("NotifySuccesssAddresses");

                return toReturn;
            }
        }

        /// <summary>
        /// Gets the environment name.
        /// </summary>
        public string NotifyEnvironmentName
        {
            get
            {
                string toReturn = Environment.GetEnvironmentVariable("NotifyEnvironmentName");

                return toReturn;
            }
        }
    }
}