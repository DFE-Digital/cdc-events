namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Describes operations for notifications of task completion.
    /// </summary>
    public interface INotifyAdapter
    {
        /// <summary>
        /// Sends a notification message.
        /// </summary>
        /// <param name="apiKey">The API key for the service.</param>
        /// <param name="emailAddress">The recipient email address.</param>
        /// <param name="templateId">The id of the template to send.</param>
        /// <param name="personalisation">The message personalisation data.</param>
        /// <param name="cancellationToken">An instance of a <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task NotifyAsync(string apiKey, string emailAddress, string templateId, Dictionary<string, dynamic> personalisation, CancellationToken cancellationToken);
    }
}