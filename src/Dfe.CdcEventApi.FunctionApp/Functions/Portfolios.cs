namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;

    /// <summary>
    /// Entry class for the <c>portfolios</c> function.
    /// </summary>
    public class Portfolios : EntityFunctionsBase
    {
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="Portfolios" />
        /// class.
        /// </summary>
        /// <param name="entityProcessor">
        /// An instance of type <see cref="IEntityProcessor" />.
        /// </param>
        /// <param name="entityArchiveProcessor">An instance of <see cref="IEntityArchiveProcessor"/>.</param>
        /// <param name="applicationStateProvider">An instance of <see cref="IApplicationStateProvider"/>.</param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public Portfolios(
            IEntityProcessor entityProcessor,
            IEntityArchiveProcessor entityArchiveProcessor,
            IApplicationStateProvider applicationStateProvider,
            ILoggerProvider loggerProvider)
            : base(
                    entityProcessor,
                    entityArchiveProcessor,
                    applicationStateProvider,
                    loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Entry method for the <c>portfolios</c> function.
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
        [FunctionName("portfolios")]
        public async Task<HttpResponseMessage> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            try
            {
                HttpResponseMessage toReturn =
                    await this.PostAsync<Portfolio>(
                        httpRequest,
                        cancellationToken)
                    .ConfigureAwait(false);

                return toReturn;
            }
            catch (System.Exception ex)
            {
                this.loggerProvider.Error($"Exception in {nameof(Portfolios)} endpoint.", ex);
                throw;
            }
        }
    }
}