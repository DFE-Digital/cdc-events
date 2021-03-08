namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>sites</c> function.
    /// </summary>
    public class Sites : FunctionsBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Sites" /> class.
        /// </summary>
        /// <param name="entityProcessor">
        /// An instance of type <see cref="IEntityProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public Sites(
            IEntityProcessor entityProcessor,
            ILoggerProvider loggerProvider)
            : base(
                  entityProcessor,
                  loggerProvider)
        {
            // Nothing for now.
        }

        /// <summary>
        /// Entry method for the <c>sites</c> function.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of type <see cref="HttpRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="IActionResult" />.
        /// </returns>
        [FunctionName("sites")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            IActionResult toReturn =
                await this.RunAsync<Site>(
                    httpRequest,
                    cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }
    }
}