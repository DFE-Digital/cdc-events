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
    /// Base class for the <see cref="Control"/> function.
    /// </summary>
    public class Control : ControlFunctionsBase
    {
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="Control" /> class.
        /// </summary>
        /// <param name="controlProcessor">
        /// An instance of type <see cref="IControlProcessor" />.
        /// </param>
        /// <param name="notifyProcessor"></param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public Control(
            IControlProcessor controlProcessor,
            INotifyProcessor notifyProcessor,
            ILoggerProvider loggerProvider)
            : base(
                  controlProcessor,
                  notifyProcessor,
                  loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Entry method for the <see cref="Control"/> function.
        /// Creates and returns a control event record.
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
        [FunctionName("control")]
        public async Task<HttpResponseMessage> ControlAsync(
            [HttpTrigger(AuthorizationLevel.Function, "POST", "PATCH", "PUT")]
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (httpRequest?.Method ?? "UNSUPPORTED")
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
                this.loggerProvider.Error($"Exception in {nameof(Control)} {httpRequest?.Method ?? "UNSUPPORTED"} endpoint.", ex);
                throw;
            }
        }
    }
}