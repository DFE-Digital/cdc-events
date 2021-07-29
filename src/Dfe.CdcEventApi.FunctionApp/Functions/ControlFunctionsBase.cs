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
        private readonly INotifyProcessor notifyProcessor;

        /// <summary>
        /// Initialises a new instance of the <see cref="ControlFunctionsBase"/> class.
        /// </summary>
        /// <param name="loggerProvider">An instance of type <see cref="IControlProcessor"/>.
        /// </param>
        /// <param name="controlProcessor">
        /// An instance of type <see cref="ILoggerProvider"/>.
        /// </param>
        /// <param name="notifyProcessor">An instance of <see cref="INotifyProcessor"/>.</param>
        public ControlFunctionsBase(
            IControlProcessor controlProcessor,
            INotifyProcessor notifyProcessor,
            ILoggerProvider loggerProvider)
            : base(loggerProvider)
        {
            this.loggerProvider = loggerProvider;
            this.controlProcessor = controlProcessor;
            this.notifyProcessor = notifyProcessor;
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
            this.loggerProvider.Info($"");
            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                this.loggerProvider.Info($"");
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);
            this.loggerProvider.Info($"");

            if (runIdentifier.HasValue)
            {
                try
                {
                    this.loggerProvider.Info($"");
                    var loads = await this.controlProcessor.CreateAsync(
                                        runIdentifier.Value,
                                        cancellationToken)
                                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"{nameof(this.controlProcessor.CreateAsync)} processed.");

                    // Everything good? Return accepted.
                    this.loggerProvider.Info($"");
                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Created);

                    // Also return the run identifier as a header.
                    this.loggerProvider.Info($"");
                    toReturn.Headers.Add(
                        HeaderNameRunIdentifier,
                        $"{loads.First().Load_DateTime:O}");

                    var sinceDateTime = (loads.LastOrDefault()?.Load_DateTime ?? this.dafaultSinceDate).AddMilliseconds(1);

                    var currentLoad = loads.First();
                    currentLoad.Since_DateTime = sinceDateTime;
                    this.loggerProvider.Info($"");
                    await this.controlProcessor.UpdateAsync(
                                currentLoad,
                                cancellationToken).ConfigureAwait(false);

                    // also return the previous run time
                    this.loggerProvider.Info($"");
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
            this.loggerProvider.Info($"");
            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                this.loggerProvider.Info($"");
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);
            this.loggerProvider.Info($"");

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
                    this.loggerProvider.Info($"");
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
                this.loggerProvider.Info($"");
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            this.loggerProvider.Info($"");
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
            this.loggerProvider.Info($"");
            if (httpRequest == null)
            {
                this.loggerProvider.Info($"");
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            // extract the Header for the runIdentifier
            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);
            this.loggerProvider.Info($"");

            // extract the Header for the status
            int? status = this.GetStatus(headerDictionary);
            this.loggerProvider.Info($"");

            if (status.HasValue && runIdentifier.HasValue)
            {
                // update the record to the specified status
                this.loggerProvider.Info($"");
                await this.controlProcessor.UpdateStatusAsync(
                    runIdentifier.Value,
                    status.Value,
                    cancellationToken)
                    .ConfigureAwait(false);

                // retrieve the record as it stands
                this.loggerProvider.Info($"");
                var load = await this.controlProcessor.GetAsync(
                                        runIdentifier.Value,
                                        cancellationToken)
                                        .ConfigureAwait(false);

                if (load.Status == ControlState.Delivered)
                {
                    load.Finished_DateTime = DateTime.UtcNow;
                    this.loggerProvider.Info($"");
                    load.Count = await this.controlProcessor.GetCountAsync(
                                runIdentifier.Value,
                                cancellationToken)
                                .ConfigureAwait(false);
                    this.loggerProvider.Info($"");
                    await this.controlProcessor.UpdateAsync(load, cancellationToken).ConfigureAwait(false);
                }

                this.loggerProvider.Info($"");
                await this.notifyProcessor.Notify(load, cancellationToken).ConfigureAwait(false);

                toReturn = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(load)),
                };
            }
            else
            {
                this.loggerProvider.Info($"");
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            this.loggerProvider.Info($"");
            return toReturn;
        }
    }
}