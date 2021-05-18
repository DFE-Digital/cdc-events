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
    /// Base class for the <c>Load</c> function.
    /// </summary>
    public class Load : LoadFunctionsBase
    {
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
            // Nothing for now.
        }

        /// <summary>
        /// Entry method for the <c>load</c> post function.
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
        [FunctionName("startload")]
        public async Task<HttpResponseMessage> StartLoadAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage toReturn =
                await this.StartLoad(
                    httpRequest,
                    cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }

        /// <summary>
        /// Updates a load control record.
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
        [FunctionName("updateload")]
        public async Task<HttpResponseMessage> UpdateLoadAsync(
            [HttpTrigger(AuthorizationLevel.Function, "PUT")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage toReturn =
                await this.UpdateLoad(
                    httpRequest,
                    cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }

        /// <summary>
        /// Finishes a load control record.
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
        [FunctionName("finishload")]
        public async Task<HttpResponseMessage> FinishLoadAsync(
            [HttpTrigger(AuthorizationLevel.Function, "GET")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage toReturn =
                await this.FinishLoad(
                    httpRequest,
                    cancellationToken)
                .ConfigureAwait(false);

            return toReturn;
        }
    }
}