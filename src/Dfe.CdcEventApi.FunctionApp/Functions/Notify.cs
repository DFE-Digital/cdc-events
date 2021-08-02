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
    /// Allows sending of notifications via the GOV.UK.Notify service, on completion of a task.
    /// </summary>
    public class Notify : NotifyFunctionBase
    {
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="Notify"/> class.
        /// </summary>
        /// <param name="notifyProcessor">
        /// An instance of type <see cref="INotifyProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public Notify(
            INotifyProcessor notifyProcessor,
            ILoggerProvider loggerProvider)
            : base(notifyProcessor, loggerProvider)
        {
            this.loggerProvider = loggerProvider;
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
        [FunctionName("notify")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "POST")] HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (httpRequest?.Method ?? "UNSUPPORTED")
                {
                    case "POST":
                        HttpResponseMessage postReturn =
                              await this.PostAsync(
                                  httpRequest,
                                  cancellationToken)
                              .ConfigureAwait(false);
                        return postReturn;
                    default:
                        return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }
            catch (System.Exception ex)
            {
                this.loggerProvider.Error($"Exception in {nameof(Attachments)} {httpRequest?.Method ?? "UNSUPPORTED"} endpoint.", ex);
                throw;
            }
        }
    }
}
