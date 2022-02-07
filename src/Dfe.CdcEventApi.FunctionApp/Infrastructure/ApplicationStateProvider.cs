namespace Dfe.CdcEventApi.FunctionApp.Infrastructure
{
    using Dfe.CdcEventApi.Domain.Definitions;

    /// <summary>
    /// In-memory application state provider.
    /// </summary>
    public class ApplicationStateProvider : IApplicationStateProvider
    {
        private int requestCount;

        /// <inheritdoc/>
        public int RequestCount
        {
            get
            {
                return this.requestCount;
            }

            set
            {
                this.requestCount = value;
            }
        }
    }
}
