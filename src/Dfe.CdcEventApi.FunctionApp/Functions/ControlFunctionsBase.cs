namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Dfe.CdcEventApi.Domain.Models;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract base class for all control functions.
    /// </summary>
    public abstract class ControlFunctionsBase : FunctionBase
    {
        private readonly ILoggerProvider loggerProvider;
        private readonly IControlProcessor controlProcessor;
        private readonly DateTime dafaultSinceDate = new DateTime(2000, 1, 1);

        /// <summary>
        /// Initialises a new instance of the <see cref="ControlFunctionsBase"/> class.
        /// </summary>
        /// <param name="loggerProvider">An instance of type <see cref="IControlProcessor"/>.
        /// </param>
        /// <param name="controlProcessor">
        /// An instance of type <see cref="ILoggerProvider"/>.
        /// </param>
        public ControlFunctionsBase(
            IControlProcessor controlProcessor,
            ILoggerProvider loggerProvider)
            : base(loggerProvider)
        {
            this.loggerProvider = loggerProvider;
            this.controlProcessor = controlProcessor;
        }

        /// <summary>
        /// Method for starting a load.
        /// The control instance is started by simply passing the Run Identifier date and time in the header.
        /// </summary>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpResponseMessage" />.
        /// The returned runIdentifier header is used to refresh the client with the actually stored date and time.
        /// </returns>
        protected async Task<HttpResponseMessage> StartLoad(
           HttpRequest httpRequest,
           CancellationToken cancellationToken)
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
                    var loads = await this.controlProcessor.CreateAsync(
                                        runIdentifier.Value,
                                        cancellationToken)
                                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"{nameof(this.controlProcessor.CreateAsync)} processed.");

                    // Everything good? Return accepted.
                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Created);

                    // Also return the run identifier as a header.
                    toReturn.Headers.Add(
                        HeaderNameRunIdentifier,
                        $"{loads.First().Load_DateTime:O}");

                    var sinceDateTime = (loads.LastOrDefault()?.Load_DateTime ?? this.dafaultSinceDate).AddMilliseconds(1);

                    var currentLoad = loads.First();
                    currentLoad.Since_DateTime = sinceDateTime;
                    await this.controlProcessor.UpdateAsync(
                                currentLoad,
                                cancellationToken).ConfigureAwait(false);

                    // also return the previous run time
                    toReturn.Headers.Add(HeaderNameSince, $"{sinceDateTime:O}");
                }
                catch (MissingLoadHandlerFileException exception)
                {
                    toReturn = new HttpResponseMessage(
                        HttpStatusCode.NotImplemented);

                    this.loggerProvider.Error(
                        $"A {nameof(MissingLoadHandlerFileException)} was " +
                        $"thrown, returning " +
                        $"{HttpStatusCode.NotImplemented}. Message: " +
                        $"{exception.Message}",
                        exception);
                }
            }
            else
            {
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return toReturn;
        }

        /// <summary>
        /// Method for updating a load. The current load status is passed as a header.
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
        protected async Task<HttpResponseMessage> UpdateLoad(
           HttpRequest httpRequest,
           CancellationToken cancellationToken)
        {
            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            int? status = this.GetStatus(headerDictionary);

            if (status.HasValue && runIdentifier.HasValue)
            {
                try
                {
                    await this.controlProcessor.UpdateStatusAsync(
                                                        runIdentifier.Value,
                                                        status.Value,
                                                        cancellationToken)
                                                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"{nameof(this.controlProcessor.UpdateStatusAsync)} processed.");

                    // Everything good? Return ok.
                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Accepted);
                }
                catch (MissingLoadHandlerFileException exception)
                {
                    toReturn = new HttpResponseMessage(
                        HttpStatusCode.NotImplemented);

                    this.loggerProvider.Error(
                        $"A {nameof(MissingLoadHandlerFileException)} was " +
                        $"thrown, returning " +
                        $"{HttpStatusCode.NotImplemented}. Message: " +
                        $"{exception.Message}",
                        exception);
                }
            }
            else
            {
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return toReturn;
        }

        /// <summary>
        /// Method to finish a load and perform completion tasks.
        /// </summary>
        /// <param name="httpRequest">
        /// the <see cref="HttpRequest"/> being processed.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronsous processing <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task{HttpResponseMessage}"/> representing the result of the asynchronous operation.
        /// </returns>
        protected async Task<HttpResponseMessage> FinishLoad(
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage toReturn;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            // extract the Header for the runIdentifier
            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            // extract the Header for the status
            int? status = this.GetStatus(headerDictionary);

            if (status.HasValue && runIdentifier.HasValue)
            {
                await this.controlProcessor.UpdateStatusAsync(
                    runIdentifier.Value,
                    status.Value,
                    cancellationToken)
                    .ConfigureAwait(false);

                var load = await this.controlProcessor.GetAsync(
                                        runIdentifier.Value,
                                        cancellationToken)
                                        .ConfigureAwait(false);

                load.Status = (ControlState)Enum.Parse(typeof(ControlState), $"{status}");
                load.Finished_DateTime = DateTime.UtcNow;
                load.Count = await this.controlProcessor.GetCountAsync(
                            runIdentifier.Value,
                            cancellationToken)
                            .ConfigureAwait(false);

                // update the load record back to its current state with report and audience.
                await this.controlProcessor.UpdateAsync(load, cancellationToken).ConfigureAwait(false);

                toReturn = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(load)),
                };
            }
            else
            {
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return toReturn;
        }
    }
}