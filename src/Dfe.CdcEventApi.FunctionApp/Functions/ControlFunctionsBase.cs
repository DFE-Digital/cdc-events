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
        protected async Task<HttpResponseMessage> StartLoad(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            this.loggerProvider.Info($"{nameof(this.StartLoad)} {httpRequest?.Method ?? string.Empty} called.");

            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                this.loggerProvider.Info($"The HTTP request object was null.");
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            if (runIdentifier.HasValue)
            {
                this.loggerProvider.Info($"The run indentifier is {runIdentifier.Value}");
                try
                {
                    this.loggerProvider.Info($"Creating the control record.");
                    var loads = await this.controlProcessor.CreateAsync(
                                        runIdentifier.Value,
                                        cancellationToken)
                                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"{nameof(this.controlProcessor.CreateAsync)} processed.");

                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Created);

                    // Also return the run identifier as a header.
                    this.loggerProvider.Info($"Returning SQL adjusted run identifier value.");
                    toReturn.Headers.Add(HeaderNameRunIdentifier, $"{loads.First().Load_DateTime:O}");

                    var sinceDateTime = (loads.LastOrDefault()?.Load_DateTime ?? this.dafaultSinceDate).AddMilliseconds(1);

                    var currentLoad = loads.First();
                    currentLoad.Since_DateTime = sinceDateTime;

                    this.loggerProvider.Info($"Updating the derived Since date and time to the created control record.");
                    await this.controlProcessor.UpdateAsync(
                                currentLoad,
                                cancellationToken).ConfigureAwait(false);

                    // also return the previous run time
                    this.loggerProvider.Info($"Adding the since date and time to the headers.");
                    toReturn.Headers.Add(HeaderNameSince, $"{sinceDateTime:O}");
                }
                catch (Exception exception)
                {
                    this.loggerProvider.Error(
                        $"A {exception.GetType()} was " +
                        $"thrown, returning " +
                        $"{HttpStatusCode.InternalServerError}. Message: " +
                        $"{exception.Message}",
                        exception);
                    toReturn = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    throw;
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
        protected async Task<HttpResponseMessage> UpdateLoad(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            this.loggerProvider.Info($"{nameof(this.UpdateLoad)} {httpRequest?.Method ?? string.Empty} called.");

            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                this.loggerProvider.Info($"The HTTP request object was null.");
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            int? status = this.GetStatus(headerDictionary);

            if (status.HasValue && runIdentifier.HasValue)
            {
                this.loggerProvider.Info($"The run indentifier is {runIdentifier.Value} the status is {status}");
                try
                {
                    await this.controlProcessor.UpdateStatusAsync(
                                                        runIdentifier.Value,
                                                        status.Value,
                                                        cancellationToken)
                                                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"{nameof(this.controlProcessor.UpdateStatusAsync)} processed.");

                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Accepted);
                }
                catch (Exception exception)
                {
                    this.loggerProvider.Error(
                        $"A {exception.GetType()} was " +
                        $"thrown, returning " +
                        $"{HttpStatusCode.InternalServerError}. Message: " +
                        $"{exception.Message}",
                        exception);
                    toReturn = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    throw;
                }
            }
            else
            {
                this.loggerProvider.Info($"The required values were not provided.");
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
            this.loggerProvider.Info($"{nameof(this.FinishLoad)} {httpRequest?.Method ?? string.Empty} called.");

            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                this.loggerProvider.Info($"The HTTP request object was null.");
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            int? status = this.GetStatus(headerDictionary);

            if (status.HasValue && runIdentifier.HasValue)
            {
                this.loggerProvider.Info($"The run indentifier is {runIdentifier.Value} the status is {status}");
                try
                {
                    // update the record to the specified status
                    this.loggerProvider.Info($"Updating the control status.");
                    await this.controlProcessor.UpdateStatusAsync(
                        runIdentifier.Value,
                        status.Value,
                        cancellationToken)
                        .ConfigureAwait(false);

                    // retrieve the record as it stands
                    this.loggerProvider.Info($"Retrieving the current control values.");
                    var load = await this.controlProcessor.GetAsync(
                                            runIdentifier.Value,
                                            cancellationToken)
                                            .ConfigureAwait(false);

                    if (load.Status == ControlState.Delivered)
                    {
                        load.Finished_DateTime = DateTime.UtcNow;

                        this.loggerProvider.Info($"Retrieving the delivered control record counts.");
                        load.Count = await this.controlProcessor.GetCountAsync(
                                    runIdentifier.Value,
                                    cancellationToken)
                                    .ConfigureAwait(false);
                    }

                    string message = this.GetMessage(headerDictionary);
                    load.Message = message;

                    this.loggerProvider.Info($"Updating the control record.");
                    await this.controlProcessor.UpdateAsync(load, cancellationToken).ConfigureAwait(false);

                    this.loggerProvider.Info($"Issuing status notification messages");
                    await this.notifyProcessor.Notify(load, cancellationToken).ConfigureAwait(false);

                    toReturn = new HttpResponseMessage(HttpStatusCode.Accepted)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(load)),
                    };
                }
                catch (Exception exception)
                {
                    this.loggerProvider.Error(
                        $"A {exception.GetType()} was " +
                        $"thrown, returning " +
                        $"{HttpStatusCode.InternalServerError}. Message: " +
                        $"{exception.Message}",
                        exception);
                    toReturn = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                    throw;
                }
            }
            else
            {
                this.loggerProvider.Info($"The required values were not provided.");
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return toReturn;
        }
    }
}