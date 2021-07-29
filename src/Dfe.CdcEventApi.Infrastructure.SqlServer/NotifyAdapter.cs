namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Notify.Client;

    /// <summary>
    /// Implements <see cref="INotifyAdapter"/>.
    /// </summary>
    public class NotifyAdapter : INotifyAdapter
    {
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="NotifyAdapter"/> class.
        /// </summary>
        /// <param name="loggerProvider">An instance of <see cref="ILoggerProvider"/>.</param>
        public NotifyAdapter(ILoggerProvider loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

        /// <inheritdoc/>
        public Task Notify(string apiKey, string emailAddress, string templateId, Dictionary<string, dynamic> personalisation)
        {
            var client = new NotificationClient(apiKey);
            this.loggerProvider.Debug($"Sending notification to {emailAddress} for {templateId}");
            client.SendEmail(emailAddress, templateId, personalisation, null, null);
            return Task.FromResult(0);
        }
    }
}