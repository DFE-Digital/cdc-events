namespace Dfe.CdcEventApi.Application.Definitions
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Defines notifiation of teask completion.
    /// </summary>
    public interface INotifyProcessor
    {
        /// <summary>
        /// Notifies recipients of the termination of a task.
        /// </summary>
        /// <param name="control">The control item for the task.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task NotifyAsync(Control control, CancellationToken cancellationToken);
    }
}