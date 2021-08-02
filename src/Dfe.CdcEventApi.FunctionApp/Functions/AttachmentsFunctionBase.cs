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
    public abstract class AttachmentsFunctionBase : FunctionBase
    {
        private readonly IAttachmentProcessor attachmentProcessor;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="AttachmentsFunctionBase" />
        /// class.
        /// </summary>
        /// <param name="attachmentProcessor">
        /// An instance of type <see cref="IAttachmentProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public AttachmentsFunctionBase(
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
            var attachtments = await this.attachmentProcessor
                                            .GetAsync(cancellationToken)
                                            .ConfigureAwait(false);

            toReturn = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(attachtments)),
            };

            return toReturn;
        }

        /// <summary>
        /// Creates an attachment file and updates the database metadata about the file.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpResponseMessage" />.
        /// </returns>
        protected async Task<HttpResponseMessage> PostAsync(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            if (runIdentifier.HasValue)
            {
                try
                {
                    string body = await httpRequest.ReadAsStringAsync()
                    .ConfigureAwait(false);

                    this.loggerProvider.Debug(
                        $"Deserialising received body: into an array " +
                        $"of {nameof(IEnumerable<AttachmentResponse>)} instance(s)...");

                    var models = JsonConvert.DeserializeObject<IEnumerable<AttachmentResponse>>(body);
                    this.loggerProvider.Info(
                        $"{models.Count()} {nameof(AttachmentResponse)} instance(s) " +
                        $"deserialised.");

                    this.loggerProvider.Debug(
                        $"Passing {models.Count()} entities to the entity " +
                        $"processor...");

                    await this.attachmentProcessor.PostAsync(
                                            runIdentifier.Value,
                                            models,
                                            cancellationToken)
                                            .ConfigureAwait(false);

                    this.loggerProvider.Info($"All {models.Count()} entities processed.");

                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Created);

                    toReturn.Headers.Add(
                        HeaderNameRunIdentifier,
                        $"{runIdentifier.Value}");
                }
                catch (MissingDataHandlerAttributeException missingDataHandlerAttributeException)
                {
                    toReturn = new HttpResponseMessage(
                        HttpStatusCode.NotImplemented);

                    this.loggerProvider.Error(
                        $"A {nameof(MissingDataHandlerAttributeException)} " +
                        $"was thrown, returning " +
                        $"{HttpStatusCode.NotImplemented}. Message: " +
                        $"{missingDataHandlerAttributeException.Message}",
                        missingDataHandlerAttributeException);
                }
                catch (MissingDataHandlerFileException missingDataHandlerFileException)
                {
                    toReturn = new HttpResponseMessage(
                        HttpStatusCode.NotImplemented);

                    this.loggerProvider.Error(
                        $"A {nameof(MissingDataHandlerFileException)} was " +
                        $"thrown, returning " +
                        $"{HttpStatusCode.NotImplemented}. Message: " +
                        $"{missingDataHandlerFileException.Message}",
                        missingDataHandlerFileException);
                }
            }
            else
            {
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);

                this.loggerProvider.Error(
                    $"A valid {nameof(runIdentifier)} was not supplied. The " +
                    $"{nameof(runIdentifier)} should either not be " +
                    $"specified, or be a valid {nameof(DateTime)} value.");
            }

            return toReturn;
        }
    }
}