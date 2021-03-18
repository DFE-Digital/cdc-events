namespace Dfe.CdcEventApi.FunctionApp.Infrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Azure.WebJobs.Host;
    using Microsoft.Extensions.Logging;
    using ILoggerProvider = Dfe.CdcEventApi.Domain.Definitions.ILoggerProvider;

    /// <summary>
    /// Implements <see cref="ILoggerProvider" />.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LoggerProvider : ILoggerProvider
    {
        private readonly ILogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="LoggerProvider" />
        /// class.
        /// </summary>
        /// <param name="logger">
        /// An instance of <see cref="TraceWriter" />.
        /// </param>
        public LoggerProvider(ILogger logger)
        {
            this.logger = logger;
        }

        /// <inheritdoc />
        public void Debug(string message)
        {
            this.logger.LogDebug(message);
        }

        /// <inheritdoc />
        public void Info(string message)
        {
            this.logger.LogInformation(message);
        }

        /// <inheritdoc />
        public void Warning(string message, Exception exception = null)
        {
            this.logger.LogWarning(message, exception);
        }

        /// <inheritdoc />
        public void Error(string message, Exception exception = null)
        {
            this.logger.LogError(message, exception);
        }
    }
}