namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Exceptions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Dfe.CdcEventApi.Domain.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract base class for all entity based functions.
    /// </summary>
    public abstract class AttachmentsForDeletionFunctionBase : FunctionBase
    {
        private readonly IAttachmentProcessor attachmentProcessor;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="AttachmentsForDeletionFunctionBase" />
        /// class.
        /// </summary>
        /// <param name="attachmentProcessor">
        /// An instance of type <see cref="IAttachmentProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public AttachmentsForDeletionFunctionBase(
            IAttachmentProcessor attachmentProcessor,
            ILoggerProvider loggerProvider)
            : base(loggerProvider)
        {
            this.attachmentProcessor = attachmentProcessor;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Gets the required attachments collection.
        /// </summary>
        /// <param name="httpRequest">
        /// The <see cref="HttpRequest"/> being processed.</param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="HttpResponseMessage"/>.
        /// </returns>
        protected async Task<HttpResponseMessage> GetAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            HttpResponseMessage toReturn;
            var attachments = await this.attachmentProcessor
                                            .GetForDeletionAsync(cancellationToken)
                                            .ConfigureAwait(false);

            toReturn = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(attachments)),
            };

            return toReturn;
        }
    }
}