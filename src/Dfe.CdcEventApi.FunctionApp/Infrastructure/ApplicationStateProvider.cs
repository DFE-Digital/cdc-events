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
        private ApplicationState applicationState = new ApplicationState();

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
            this.applicationState.RunIdentifier = runIdentifier;
            this.applicationState.ForceResponseStatusCode = string.Empty;
            this.applicationState.ForceResponseStatusCodeCount = 0;
            this.RequestCount = 0;
        }

        /// <inheritdoc/>
        public bool ForceResponseStatusCode(string statusCode, string responseCount)
        {
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

            this.RequestCount++;

            return returnVal;
        }
    }
}
