namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry point for the <see cref="Attachments"/> function.
    /// </summary>
    public class Attachments : LoadFunctionsBase
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Attachments" /> class.
        /// </summary>
        /// <param name="loadProcessor">
        /// An instance of type <see cref="IEntityProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public Attachments(
            ILoadProcessor loadProcessor,
            ILoggerProvider loggerProvider)
            : base(loadProcessor, loggerProvider)
        {
            // nothing here.
        }

        /// <summary>
        /// Entry method for the <c>attachments</c> function.
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
        [FunctionName("attachments")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET")] HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage toReturn =
                await this.GetAttachments(
                    httpRequest,
                    cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }
    }
}
