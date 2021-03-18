namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Exceptions;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// Abstract base class for all functions.
    /// </summary>
    public abstract class FunctionsBase
    {
        private readonly IEntityProcessor entityProcessor;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="FunctionsBase" />
        /// class.
        /// </summary>
        /// <param name="entityProcessor">
        /// An instance of type <see cref="IEntityProcessor" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public FunctionsBase(
            IEntityProcessor entityProcessor,
            ILoggerProvider loggerProvider)
        {
            this.entityProcessor = entityProcessor;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Base entry method for all functions.
        /// </summary>
        /// <typeparam name="TModelsBase">
        /// A type deriving from <see cref="ModelsBase" />.
        /// </typeparam>
        /// <param name="httpRequest">
        /// An instance of <see cref="HttpRequest" />.
        /// </param>
        /// <param name="cancellationToken">
        /// An instance of <see cref="CancellationToken" />.
        /// </param>
        /// <returns>
        /// An instance of <see cref="HttpResponseMessage" />.
        /// </returns>
        protected async Task<HttpResponseMessage> RunAsync<TModelsBase>(
            HttpRequest httpRequest,
            CancellationToken cancellationToken)
            where TModelsBase : ModelsBase
        {
            HttpResponseMessage toReturn = null;

            string body = await httpRequest.ReadAsStringAsync()
                .ConfigureAwait(false);

            string modelsBaseType = typeof(TModelsBase).Name;

            this.loggerProvider.Debug(
                $"Deserialising received body: \"{body}\" into an array of " +
                $"{modelsBaseType} instance(s)...");

            IEnumerable<TModelsBase> modelsBases =
                JsonConvert.DeserializeObject<IEnumerable<TModelsBase>>(body);

            this.loggerProvider.Info(
                $"{modelsBases.Count()} {modelsBaseType} instance(s) " +
                $"deserialised.");

            this.loggerProvider.Debug(
                $"Passing {modelsBases.Count()} entities to the entity " +
                $"processor...");

            try
            {
                await this.entityProcessor.ProcessEntitiesAsync(
                    modelsBases,
                    cancellationToken)
                    .ConfigureAwait(false);

                this.loggerProvider.Info(
                    $"All {modelsBases.Count()} entities processed.");

                // Everything good? Return accepted.
                toReturn = new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            catch (MissingDataHandlerAttributeException missingDataHandlerAttributeException)
            {
                toReturn = new HttpResponseMessage(
                    HttpStatusCode.NotImplemented);

                this.loggerProvider.Error(
                    $"A {nameof(MissingDataHandlerAttributeException)} was " +
                    $"thrown, returning {HttpStatusCode.NotImplemented}. " +
                    $"Message: {missingDataHandlerAttributeException.Message}",
                    missingDataHandlerAttributeException);
            }
            catch (MissingDataHandlerFileException missingDataHandlerFileException)
            {
                toReturn = new HttpResponseMessage(
                    HttpStatusCode.NotImplemented);

                this.loggerProvider.Error(
                    $"A {nameof(MissingDataHandlerFileException)} was " +
                    $"thrown, returning {HttpStatusCode.NotImplemented}. " +
                    $"Message: {missingDataHandlerFileException.Message}",
                    missingDataHandlerFileException);
            }

            return toReturn;
        }
    }
}