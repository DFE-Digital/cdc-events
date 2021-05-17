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
    using Dfe.CdcEventApi.Application;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Exceptions;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract base class for all load control functions.
    /// </summary>
    public abstract class LoadControlFunctionsBase
    {
        private const string HeaderNameRunIdentifier = "X-Run-Identifier";

        private readonly ILoggerProvider loggerProvider;
        private readonly ILoadControlProcessor loadControlProcessor;



        /// <summary>
        /// Initialises a new instance of the <see cref="LoadControlFunctionsBase"/> class.
        /// </summary>
        /// <param name="loggerProvider">An instance of type <see cref="ILoadControlProcessor"/>.
        /// </param>
        /// <param name="loadControlProcessor">
        /// An instance of type <see cref="ILoggerProvider"/>.
        /// </param>
        public LoadControlFunctionsBase(ILoadControlProcessor loadControlProcessor, ILoggerProvider loggerProvider)
        {
            this.loggerProvider = loggerProvider;
            this.loadControlProcessor = loadControlProcessor;
        }

        protected async Task<HttpResponseMessage> PostLoadControlAsync(
           HttpRequest httpRequest,
           CancellationToken cancellationToken)
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
                    runIdentifier = DateTime.Parse(
                        runIdentifierStr,
                        CultureInfo.InvariantCulture);
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
                string body = await httpRequest.ReadAsStringAsync()
                    .ConfigureAwait(false);

                string modelsBaseType = typeof(LoadControl).Name;

                this.loggerProvider.Debug(
                    $"Deserialising received body: \"{body}\" into an array " +
                    $"of {modelsBaseType} instance(s)...");




                try
                {
                    var loadControl = await this.loadControlProcessor.CreateLoadControlAsync(
                        runIdentifier.Value,
                        cancellationToken)
                        .ConfigureAwait(false);

                    this.loggerProvider.Info(
                        $"CreateLoadControlAsync processed.");

                    // Everything good? Return accepted.
                    toReturn =
                        new HttpResponseMessage(HttpStatusCode.Created);

                    // Also return the run identifier.
                    toReturn.Headers.Add(
                        HeaderNameRunIdentifier,
                        $"{loadControl.Load_DateTime:u}");
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

        protected async Task<HttpResponseMessage> GetAsync()
        {

        }


        protected async Task<HttpResponseMessage> PutAsync()
        {

        }


        protected async Task<HttpResponseMessage> DeleteAsync()
        {

        }
    }
}