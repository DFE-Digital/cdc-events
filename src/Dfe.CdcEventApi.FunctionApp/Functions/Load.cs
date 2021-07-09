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
    /// Base class for the <see cref="Load"/> function.
    /// </summary>
    public class Load : LoadFunctionsBase
    {
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="Load" /> class.
        /// </summary>
        /// <param name="loadProcessor">
        /// An instance of type <see cref="ILoadProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public Load(
            ILoadProcessor loadProcessor,
            ILoggerProvider loggerProvider)
            : base(
                  loadProcessor,
                  loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Entry method for the <c>load</c> function.
        /// Creates and returns a load event record.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of type <see cref="HttpRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of type <see cref="HttpResponseMessage" />.
        /// </returns>
        [FunctionName("load")]
        public async Task<HttpResponseMessage> LoadAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST", "PATCH", "PUT")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (httpRequest?.Method ?? "BAD")
                {
                    case "POST":
                        HttpResponseMessage postReturn =
                                                await this.StartLoad(
                                                    httpRequest,
                                                    cancellationToken)
                                                .ConfigureAwait(false);

                        return postReturn;

                    case "PATCH":

                        HttpResponseMessage patchReturn =
                                                await this.UpdateLoad(
                                                    httpRequest,
                                                    cancellationToken)
                                                .ConfigureAwait(false);

                        return patchReturn;

                    case "PUT":
                        HttpResponseMessage putReturn =
                                                await this.FinishLoad(
                                                    httpRequest,
                                                    cancellationToken)
                                                .ConfigureAwait(false);

                        return putReturn;
                    default:
                        return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }
            catch (System.Exception ex)
            {

                this.loggerProvider.Error($"Exception in {nameof(Load)} endpoint.", ex);
                throw;
            }
        }
    }
}