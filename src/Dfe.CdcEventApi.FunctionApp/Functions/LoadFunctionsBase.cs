namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Dfe.CdcEventApi.Domain.Models;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract base class for all load control functions.
    /// </summary>
    public abstract class LoadFunctionsBase
    {
        private const string HeaderNameRunIdentifier = "X-Run-Identifier";
        private const string HeaderNameStatus = "X-Run-Status";
        private const string HeaderNameSince = "X-Run-Since";
        private readonly ILoggerProvider loggerProvider;
        private readonly ILoadProcessor loadProcessor;
        private readonly DateTime dafaultSinceDate = new DateTime(2000, 1, 1);

        /// <summary>
        /// Initialises a new instance of the <see cref="LoadFunctionsBase"/> class.
        /// </summary>
        /// <param name="loggerProvider">An instance of type <see cref="ILoadProcessor"/>.
        /// </param>
        /// <param name="loadProcessor">
        /// An instance of type <see cref="ILoggerProvider"/>.
        /// </param>
        public LoadFunctionsBase(
            ILoadProcessor loadProcessor,
            ILoggerProvider loggerProvider)
        {
            this.loggerProvider = loggerProvider;
            this.loadProcessor = loadProcessor;
        }

        /// <summary>
        /// Method for starting a load.
        /// The load is started by simply passing the Run Identifier date and time in the header.
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
                    var loads = await this.loadProcessor.CreateLoadAsync(
                                        runIdentifier.Value,
                                        cancellationToken)
                                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"{nameof(this.loadProcessor.CreateLoadAsync)} processed.");

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
                    await this.loadProcessor.UpdateLoadAsync(
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

            short? status = this.GetStatus(headerDictionary);

            if (status.HasValue && runIdentifier.HasValue)
            {
                try
                {
                    await this.loadProcessor.UpdateLoadStatusAsync(
                                                        runIdentifier.Value,
                                                        status.Value,
                                                        cancellationToken)
                                                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"{nameof(this.loadProcessor.UpdateLoadStatusAsync)} processed.");

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
            HttpResponseMessage toReturn = null;

            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            // extract the Header for the runIdentifier
            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            // extract the Header for the status
            short? status = this.GetStatus(headerDictionary);

            if (status.HasValue && runIdentifier.HasValue)
            {
                await this.loadProcessor.UpdateLoadStatusAsync(
                    runIdentifier.Value,
                    status.Value,
                    cancellationToken)
                    .ConfigureAwait(false);

                var load = await this.loadProcessor.GetLoadAsync(
                                        runIdentifier.Value,
                                        cancellationToken)
                                        .ConfigureAwait(false);

                // get the load notification data for the status
                var notifications =
                    await this.loadProcessor.GetLoadNotifications(status.Value, cancellationToken)
                    .ConfigureAwait(false);

                LoadNotificationTemplate template = await this.loadProcessor
                        .GetLoadTemplateForStatus(
                            status.Value,
                            cancellationToken)
                        .ConfigureAwait(false);

                if (template != null)
                {
                    // get the required notification evidence
                    if (template.IncludeRIChecks)
                    {
                        // nothing yet.
                        // var statistics = await this.loadProcessor.GetRIChecks(runIdentifier, cancellationToken).ConfigureAwait(false);
                    }

                    // get load stats.
                    if (template.IncludeRowStats)
                    {
                        // nothing yet.
                        // var statistics = await this.loadProcessor.GetLoadStatisticsAsync(runIdentifier, cancellationToken).ConfigureAwait(false);
                    }
                }

                LoadStates state = load.Status;

                load.ReportTitle = template.Subject.Replace(
                    "{0}",
                    state.ToEnumDescription(),
                    StringComparison.InvariantCultureIgnoreCase);

                load.ReportTo = string.Join("; ", notifications.Select(x => x.Email));

                // update the load with the report
                StringBuilder reportBody = new StringBuilder("Dummy Report");

                load.ReportBody = reportBody.ToString();
                load.Finish_DateTime = DateTime.UtcNow;

                toReturn = new HttpResponseMessage(HttpStatusCode.Accepted)
                {
                    // return the notification data to the caller.
                    Content = new StringContent(JsonConvert.SerializeObject(load)),
                };

                if (state == LoadStates.Suceeeded)
                {
                    var count = await this.loadProcessor.GetLoadCountAsync(
                        runIdentifier.Value,
                        cancellationToken)
                        .ConfigureAwait(false);
                    load.Count = count;
                }

                // finally update the load record back to its complete state with report and audience.
                await this.loadProcessor.UpdateLoadAsync(load, cancellationToken)
                        .ConfigureAwait(false);
            }
            else
            {
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the resulting attachments for a load run.
        /// </summary>
        /// <param name="httpRequest">
        /// The <see cref="HttpRequest"/> being processed.</param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="HttpRequestMessage"/>.
        /// </returns>
        protected async Task<HttpResponseMessage> GetAttachments(HttpRequest httpRequest, CancellationToken cancellationToken)
        {
            if (httpRequest == null)
            {
                throw new ArgumentNullException(nameof(httpRequest));
            }

            IHeaderDictionary headerDictionary = httpRequest.Headers;

            // extract the Header for the runIdentifier
            DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);

            HttpResponseMessage toReturn;
            if (runIdentifier.HasValue)
            {
                var attachtments = await this.loadProcessor.GetAttachments(
                                                                runIdentifier.Value,
                                                                cancellationToken).ConfigureAwait(false);

                toReturn = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(attachtments)),
                };
            }
            else
            {
                toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return toReturn;
        }

        private short? GetStatus(IHeaderDictionary headerDictionary)
        {
            short? status = null;
            this.loggerProvider.Debug($"Checking for header \"{HeaderNameStatus}\"...");

            if (headerDictionary.ContainsKey(HeaderNameStatus))
            {
                var statusString = headerDictionary[HeaderNameStatus];

                this.loggerProvider.Info(
                    $"Header \"{HeaderNameStatus}\" was specified: " +
                    $"\"{statusString}\". Parsing...");

                try
                {
                    status = short.Parse(
                        statusString,
                        CultureInfo.InvariantCulture);
                    if (
                            status.Value < (short)LoadStates.Initialising
                            ||
                            status.Value > (short)LoadStates.Suceeeded)
                    {
                        this.loggerProvider.Error(
                                $"An invalid {nameof(status)} was supplied. The " +
                                $"{nameof(status)} must be " +
                                $"specified as a valid {nameof(Int16)} value in the range 1..32.");
                        return null;
                    }
                }
                catch (FormatException formatException)
                {
                    this.loggerProvider.Warning(
                        $"Unable to parse the value of " +
                        $"\"{HeaderNameRunIdentifier}\" " +
                        $"(\"{statusString}\") as a {nameof(Int16)}.",
                        formatException);
                }

                return status;
            }
            else
            {
                this.loggerProvider.Error(
                    $"A valid {nameof(status)} was not supplied. The " +
                    $"{nameof(status)} must be " +
                    $"specified as a valid {nameof(Int16)} value in the range 1..32.");
                return null;
            }
        }

        private DateTime? GetRunIdentifier(IHeaderDictionary headerDictionary)
        {
            DateTime? runIdentifier = null;
            string runIdentifierStr = null;

            this.loggerProvider.Debug($"Checking for header \"{HeaderNameRunIdentifier}\"...");

            if (headerDictionary.ContainsKey(HeaderNameRunIdentifier))
            {
                runIdentifierStr = headerDictionary[HeaderNameRunIdentifier];

                this.loggerProvider.Info(
                    $"Header \"{HeaderNameRunIdentifier}\" was specified: " +
                    $"\"{runIdentifierStr}\". Parsing...");

                try
                {
                    runIdentifier = DateTime.Parse(
                        runIdentifierStr,
                        CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    this.loggerProvider.Error(
                        $"A valid {nameof(runIdentifier)} was not usable. The " +
                        $"{nameof(runIdentifier)} must be " +
                        $"specified as a valid {nameof(DateTime)} value.");
                    return null;
                }
            }

            return runIdentifier;
        }
    }
}