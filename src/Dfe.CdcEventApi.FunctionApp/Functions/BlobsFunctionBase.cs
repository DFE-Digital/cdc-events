namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Exceptions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Dfe.CdcEventApi.Domain.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract base class for all entity based functions.
    /// </summary>
    public abstract class BlobsFunctionBase
    {
        private const string HeaderNameRunIdentifier = "X-Run-Identifier";
        private readonly IBlobSettingsProvider blobSettingsProvider;
        private readonly IBlobProcessor blobProcessor;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="BlobsFunctionBase" />
        /// class.
        /// </summary>
        /// <param name="blobProcessor">
        /// An instance of type <see cref="IBlobProcessor" />.
        /// </param>
        /// <param name="blobSettingsProvider">
        /// An instance of <see cref="IBlobSettingsProvider"/>.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public BlobsFunctionBase(
            IBlobProcessor blobProcessor,
            IBlobSettingsProvider blobSettingsProvider,
            ILoggerProvider loggerProvider)
        {
            this.blobSettingsProvider = blobSettingsProvider;
            this.blobProcessor = blobProcessor;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Base entry method for all functions.
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

            this.loggerProvider.Debug(
                $"Checking for header \"{HeaderNameRunIdentifier}\"...");

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = null;
            string runIdentifierStr = null;
            if (headerDictionary.ContainsKey(HeaderNameRunIdentifier))
            {
                runIdentifierStr = headerDictionary[HeaderNameRunIdentifier];

                this.loggerProvider.Info(
                    $"Header \"{HeaderNameRunIdentifier}\" was specified: " +
                    $"\"{runIdentifierStr}\". Parsing...");

                try
                {
                    runIdentifier = DateTime.Parse(runIdentifierStr, CultureInfo.InvariantCulture);
                }
                catch (FormatException formatException)
                {
                    this.loggerProvider.Warning(
                        $"Unable to parse the value of " +
                        $"\"{HeaderNameRunIdentifier}\" " +
                        $"(\"{runIdentifierStr}\") as a {nameof(DateTime)}.",
                        formatException);
                }
            }

            if (string.IsNullOrEmpty(runIdentifierStr))
            {
                runIdentifier = DateTime.UtcNow;

                this.loggerProvider.Info(
                    $"Header \"{HeaderNameRunIdentifier}\" not supplied, or " +
                    $"was blank. {nameof(runIdentifierStr)} will default to " +
                    $"\"{runIdentifierStr}\".");
            }

            if (runIdentifier.HasValue)
            {
                try
                {
                    string body = await httpRequest.ReadAsStringAsync()
                    .ConfigureAwait(false);

                    this.loggerProvider.Debug(
                        $"Deserialising received body: into an array " +
                        $"of {nameof(IEnumerable<Blob>)} instance(s)...");

                    var models = JsonConvert.DeserializeObject<IEnumerable<Blob>>(body);
                    this.loggerProvider.Info(
                        $"{models.Count()} {nameof(Blob)} instance(s) " +
                        $"deserialised.");

                    this.loggerProvider.Debug(
                        $"Passing {models.Count()} entities to the entity " +
                        $"processor...");

                    await this.blobProcessor.CreateBlobsAsync(
                        runIdentifier.Value,
                        models,
                        this.blobSettingsProvider.BlobStorageConnectionString,
                        this.blobSettingsProvider.BlobStorageAccountName,
                        this.blobSettingsProvider.BlobStorageAccountKey,
                        cancellationToken)
                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"All {models.Count()} entities processed.");

                    // Everything good? Return accepted.
                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Accepted);

                    // Also return the run identifier.
                    runIdentifierStr = runIdentifier.ToString();

                    toReturn.Headers.Add(
                        HeaderNameRunIdentifier,
                        runIdentifierStr);
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