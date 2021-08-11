namespace Dfe.CdcEventApi.Application
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Implements <see cref="INotifyProcessor"/>.
    /// </summary>
    public class NotifyProcessor : INotifyProcessor
    {
        private readonly ILoggerProvider loggerProvider;
        private readonly INotifyAdapter notifyAdapter;
        private readonly string aPIKey;
        private readonly string successTemplateId;
        private readonly string failureTemplateId;
        private readonly string sucesssAddresses;
        private readonly string failureAddresses;
        private readonly string environmentName;

        /// <summary>
        /// Initialises a new instance of the <see cref="NotifyProcessor"/> class.
        /// </summary>
        /// <param name="loggerProvider">An instance of <see cref="ILoggerProvider"/>.</param>
        /// <param name="notifyAdapter">An instance of <see cref="INotifyAdapter"/>.</param>
        /// <param name="notifySettingsProvider">An instance of <see cref="INotifySettingsProvider"/>.</param>
        public NotifyProcessor(
            ILoggerProvider loggerProvider,
            INotifyAdapter notifyAdapter,
            INotifySettingsProvider notifySettingsProvider)
        {
            if (notifySettingsProvider == null)
            {
                throw new ArgumentNullException(nameof(notifySettingsProvider));
            }

            this.loggerProvider = loggerProvider;
            this.notifyAdapter = notifyAdapter;
            this.aPIKey = notifySettingsProvider.NotifyApiKey;
            this.successTemplateId = notifySettingsProvider.NotifySuccessTemplateId;
            this.failureTemplateId = notifySettingsProvider.NotifyFailureTemplateId;
            this.sucesssAddresses = notifySettingsProvider.NotifySuccesssAddresses;
            this.failureAddresses = notifySettingsProvider.NotifyFailureAddresses;
            this.environmentName = notifySettingsProvider.NotifyEnvironmentName;
        }

        /// <summary>
        /// Processes a notification request and sends the messages.
        /// </summary>
        /// <param name="control">The task control record.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        public async Task NotifyAsync(Control control, CancellationToken cancellationToken)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            this.ThrowExceptionOnMissingSettings();

            this.loggerProvider.Info($"{nameof(this.NotifyAsync)} processing control for {control.Load_DateTime}");
            var personalisation = new Dictionary<string, dynamic>();
            var success = control.Status == ControlState.Delivered;
            var successAddresses = this.sucesssAddresses.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var failureAddresses = this.failureAddresses.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var addresses = success ? successAddresses : failureAddresses;

            var templateId = success ? this.successTemplateId : this.failureTemplateId;
            if (success)
            {
                personalisation.Add("environment", this.environmentName);
                personalisation.Add("runIdentifier", control.Load_DateTime);
                personalisation.Add("runStatus", control.Status.ToEnumDescription());
                personalisation.Add("message", control.Message ?? "No message.");
            }

            foreach (var address in addresses)
            {
                this.loggerProvider.Debug($"Calling the Notify system to send a message.");
                await this.notifyAdapter.NotifyAsync(this.aPIKey.Trim(), address.Trim(), templateId.Trim(), personalisation, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        private void ThrowExceptionOnMissingSettings()
        {
            var reasons = string.Empty;
            var settings = new Dictionary<string, string>()
            {
                { "APIKey", this.aPIKey },
                { "SuccessTemplateId", this.successTemplateId },
                { "FailureTemplateId", this.failureTemplateId },
                { "SucesssAddresses", this.sucesssAddresses },
                { "FailureAddresses", this.failureAddresses },
                { "EnvironmentName", this.environmentName },
            };

            foreach (var setting in settings)
            {
                if (string.IsNullOrWhiteSpace(setting.Value))
                {
                    reasons += $"{setting.Key} has no value\r\n";
                }
            }

            if (!string.IsNullOrEmpty(reasons))
            {
                throw new ArgumentException(reasons);
            }
        }
    }
}