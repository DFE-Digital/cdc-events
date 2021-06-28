namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>blobs</c> function.
    /// </summary>
    public class Blobs : BlobsFunctionBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Blobs" /> class.
        /// </summary>
        /// <param name="blobProcessor">
        /// An instance of type <see cref="IBlobProcessor" />.
        /// </param>
        /// <param name="blobSettingsProvider">
        /// An instance of type <see cref="IBlobSettingsProvider" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public Blobs(
            IBlobProcessor blobProcessor,
            IBlobSettingsProvider blobSettingsProvider,
            ILoggerProvider loggerProvider)
            : base(blobProcessor, blobSettingsProvider, loggerProvider)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Entry method for the <c>blobs</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of type <see cref="HttpRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpResponseMessage" />.
        /// </returns>
        [FunctionName("blobs")]
        public async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {

#pragma warning disable SA1123 // Do not place regions within elements
            #region REDUNDANTCODE
#pragma warning restore SA1123 // Do not place regions within elements

            // NOTE: The code checking the connection string and account key is to diagnose problems with the new configuration requirements that have not yet been tested and can be removed in future versions
            var configAccountKey = Environment.GetEnvironmentVariable("BlobStorageAccountKey", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(configAccountKey))
            {
                throw new NullReferenceException(nameof(configAccountKey));
            }

            var configAccountConnectionString = Environment.GetEnvironmentVariable("BlobStorageConnectionString", EnvironmentVariableTarget.Process);
            if (string.IsNullOrEmpty(configAccountConnectionString))
            {
                throw new NullReferenceException(nameof(configAccountConnectionString));
            }

            if (configAccountKey.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"{nameof(configAccountKey)} has not been translated from the keyvault correctly.");
            }

            if (configAccountKey.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"{nameof(configAccountKey)} has not been translated from the keyvault correctly.");
            }
            #endregion

            HttpResponseMessage toReturn =
                await this.PostAsync(httpRequest, cancellationToken).ConfigureAwait(false);

            return toReturn;
        }
    }
}