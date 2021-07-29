namespace Dfe.CdcEventApi.FunctionApp.Infrastructure.SettingsProviders
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    public class NotifySettingsProvider : INotifySettingsProvider
    {
        public string APIKey { get; }
        public string SuccessTemplateId { get; }
        public string FailureAddresses { get; }
        public string SucesssAddresses { get; }
    }
}