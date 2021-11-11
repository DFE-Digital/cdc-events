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
    /// Entry point for the <see cref="AttachmentsForDeletion"/> function.
    /// </summary>
    public class AttachmentsForDeletion : AttachmentsFunctionBase
    {
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="AttachmentsForDeletion" /> class.
        /// </summary>
        /// <param name="attachmentProcessor">
        /// An instance of type <see cref="IAttachmentProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public AttachmentsForDeletion(
            IAttachmentProcessor attachmentProcessor,
            ILoggerProvider loggerProvider)
            : base(attachmentProcessor, loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Entry method for the <c>attachmentsfordeletion</c> function.
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
        [FunctionName("attachmentsfordeletion")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "GET")] HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            try
            {
                switch (httpRequest?.Method ?? "UNSUPPORTED")
                {
                    case "GET":
                        HttpResponseMessage getReturn =
                               await this.GetAsync(
                                   httpRequest,
                                   cancellationToken)
                               .ConfigureAwait(false);
                        return getReturn;
                    default:
                        return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
            }
            catch (System.Exception ex)
            {
                this.loggerProvider.Error($"Exception in {nameof(AttachmentsForDeletion)} {httpRequest?.Method ?? "UNSUPPORTED"} endpoint.", ex);
                throw;
            }
        }
    }
}
