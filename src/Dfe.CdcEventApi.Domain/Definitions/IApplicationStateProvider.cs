namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System;

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

        /// <summary>
        /// Resets the application state.
        /// </summary>
        /// <param name="runIdentifier">Timestamp of the new run.</param>
        void StartLoad(DateTime runIdentifier);

        /// <summary>
        /// Determines whether to force a response code for testing purposes.
        /// </summary>
        /// <param name="statusCode">The status code to return.</param>
        /// <param name="responseCount">The number of responses to force the status code for.</param>
        /// <returns>bool indicated whether to force response code.</returns>
        bool ForceResponseStatusCode(string statusCode, string responseCount);
    }
}
