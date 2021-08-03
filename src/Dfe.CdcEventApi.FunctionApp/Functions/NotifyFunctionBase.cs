namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System;
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

    /// <summary>
    /// Abstract base class for all entity based functions.
    /// </summary>
    public abstract class NotifyFunctionBase : FunctionBase
    {
        private readonly INotifyProcessor notifyProcessor;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="NotifyFunctionBase" />
        /// class.
        /// </summary>
        /// <param name="notifyProcessor">
        /// An instance of type <see cref="INotifyProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public NotifyFunctionBase(
            INotifyProcessor notifyProcessor,
            ILoggerProvider loggerProvider)
            : base(loggerProvider)
        {
            this.notifyProcessor = notifyProcessor;
            this.loggerProvider = loggerProvider;
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

            try
            {
                IHeaderDictionary headerDictionary = httpRequest.Headers;

                DateTime? runIdentifier = this.GetRunIdentifier(headerDictionary);
                DateTime? sinceDate = this.GetSince(headerDictionary);
                int? statusVal = this.GetStatus(headerDictionary);
                string message = this.GetMessage(headerDictionary);

                if (runIdentifier.HasValue && sinceDate.HasValue && statusVal.HasValue && message != null)
                {
                    var control = new Domain.Models.Control()
                    {
                        Load_DateTime = runIdentifier.Value,
                        Since_DateTime = sinceDate.Value,
                        Status = (ControlState)statusVal,
                        Message = string.Empty,
                    };

                    await this.notifyProcessor.NotifyAsync(
                                            control,
                                            cancellationToken)
                                            .ConfigureAwait(false);

                    toReturn = new HttpResponseMessage(HttpStatusCode.Created);
                }
                else
                {
                    toReturn = new HttpResponseMessage(HttpStatusCode.BadRequest);

                    this.LogNotifyHeaderRequirements();
                }
            }
            catch (MissingHeaderFileException ex)
            {
                toReturn = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                this.LogNotifyHeaderRequirements();
                this.loggerProvider.Error(
                    $"A {nameof(MissingHeaderFileException)} was " +
                    $"thrown, returning " +
                    $"{HttpStatusCode.NotImplemented}. Message: " +
                    $"{ex.Message}",
                    ex);
            }
            catch (MissingDataHandlerFileException ex)
            {
                toReturn = new HttpResponseMessage(
                    HttpStatusCode.NotImplemented);

                this.loggerProvider.Error(
                    $"A {nameof(MissingDataHandlerFileException)} was " +
                    $"thrown, returning " +
                    $"{HttpStatusCode.NotImplemented}. Message: " +
                    $"{ex.Message}",
                    ex);
            }
            catch (MissingDataHandlerAttributeException ex)
            {
                toReturn = new HttpResponseMessage(
                    HttpStatusCode.NotImplemented);

                this.loggerProvider.Error(
                    $"A {nameof(MissingDataHandlerAttributeException)} " +
                    $"was thrown, returning " +
                    $"{HttpStatusCode.NotImplemented}. Message: " +
                    $"{ex.Message}",
                    ex);
            }

            return toReturn;
        }

        /// <summary>
        /// Writes a log entry regarding header requirements.
        /// </summary>
        protected void LogNotifyHeaderRequirements()
        {
            this.loggerProvider.Error(
                        $"The following headers must be set; '{HeaderNameRunIdentifier}' and  '{HeaderNameSince}' should carry a valid date and time, '{HeaderNameStatus}' should be in the range {ControlState.Start}..{ControlState.Finished}, and '{HeaderNameMessage}' should carry a message string.");
        }
    }
}