namespace Dfe.CdcEventApi.Domain.Definitions
{
    /// <summary>
    /// Describes the operations of the <see cref="IApplicationStateProvider" />
    /// provider.
    /// </summary>
    public interface IApplicationStateProvider
    {
        /// <summary>
        /// Gets or sets the number of requests.
        /// </summary>
        int RequestCount
        {
            get;
            set;
        }
    }
}
