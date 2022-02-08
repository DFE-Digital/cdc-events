namespace Dfe.CdcEventApi.FunctionApp.Infrastructure
{
    using System;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// In-memory application state provider.
    /// </summary>
    public class ApplicationStateProvider : IApplicationStateProvider
    {
        private int requestCount;
        private ApplicationState applicationState;
        private ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationStateProvider"/> class.
        /// </summary>
        /// <param name="loggerProvider">Logger provider.</param>
        public ApplicationStateProvider(ILoggerProvider loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

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

        /// <inheritdoc/>
        public void StartLoad(DateTime runIdentifier)
        {
            this.loggerProvider.Info($"Initialising ApplicationState with RunIdentifier {runIdentifier}.");

            this.applicationState.RunIdentifier = runIdentifier;
            this.applicationState.ForceResponseStatusCode = string.Empty;
            this.applicationState.ForceResponseStatusCodeCount = 0;
            this.RequestCount = 0;
        }

        /// <inheritdoc/>
        public bool ForceResponseStatusCode(string statusCode, string responseCount)
        {
            this.loggerProvider.Info($"ForceResponseStatusCode called - statusCode: {statusCode}, responseCount: {responseCount}, requestCount: {this.RequestCount}.");

            bool returnVal = false;
            int responseCountVal;
            int statusCodeVal;

            if (!string.IsNullOrWhiteSpace(statusCode) && !string.IsNullOrWhiteSpace(responseCount))
            {
                if (int.TryParse(statusCode, out statusCodeVal) && int.TryParse(responseCount, out responseCountVal))
                {
                    returnVal = this.requestCount < responseCountVal;
                }
            }

            this.loggerProvider.Info($"ForceResponseStatusCode result {returnVal}.");

            this.RequestCount++;

            return returnVal;
        }
    }
}
