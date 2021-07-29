namespace Dfe.CdcEventApi.Application
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
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

        /// <summary>
        /// Initialises a new instance of the <see cref="NotifyProcessor"/> class.
        /// </summary>
        /// <param name="loggerProvider"></param>
        /// <param name="notifyAdapter"></param>
        /// <param name="notifySettingsProvider"></param>
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
            this.aPIKey = notifySettingsProvider.APIKey;
            this.successTemplateId = notifySettingsProvider.SuccessTemplateId;
            this.failureTemplateId = notifySettingsProvider.FailureAddresses;
            this.sucesssAddresses = notifySettingsProvider.SucesssAddresses;
            this.failureAddresses = notifySettingsProvider.FailureAddresses;
        }

        /// <summary>
        /// Processes a notification request and sends the messages.
        /// </summary>
        /// <param name="control">The task control record.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        public async Task Notify(Control control, CancellationToken cancellationToken)
        {
            if (control == null)
            {
                throw new ArgumentNullException(nameof(control));
            }

            this.loggerProvider.Info($"{nameof(this.Notify)} processing control for {control.Load_DateTime}");
            var personalisation = new Dictionary<string, dynamic>();
            var success = control.Status == ControlState.Delivered;
            var addresses = success ? this.sucesssAddresses.Split(';') : this.failureAddresses.Split(';');
            var templateId = success ? this.successTemplateId : this.failureTemplateId;
            if (success)
            {
                personalisation.Add("", "");
            }

            foreach (var address in addresses)
            {
                this.loggerProvider.Debug($"");
                await this.notifyAdapter.Notify(this.aPIKey, address, templateId, personalisation)
                    .ConfigureAwait(false);
            }
        }
    }
}