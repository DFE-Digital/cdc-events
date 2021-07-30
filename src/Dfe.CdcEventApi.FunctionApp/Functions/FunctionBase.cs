namespace Dfe.CdcEventApi.FunctionApp.Functions
{
    using System;
    using System.Globalization;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Models;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// The base class for all function Base classes.
    /// </summary>
    public abstract class FunctionBase
    {
        /// <summary>
        /// The name of the Run Identifier header.
        /// </summary>
        protected const string HeaderNameRunIdentifier = "X-Run-Identifier";

        /// <summary>
        /// The name of the Status header.
        /// </summary>
        protected const string HeaderNameStatus = "X-Run-Status";

        /// <summary>
        /// The name of theSince header.
        /// </summary>
        protected const string HeaderNameSince = "X-Run-Since";

        /// <summary>
        /// The name of the Message header.
        /// </summary>
        protected const string HeaderNameMessage = "X-Run-Messsage";

        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="FunctionBase"/> class.
        /// </summary>
        /// <param name="loggerProvider">An instance of <see cref="ILoggerProvider"/>.</param>
        public FunctionBase(ILoggerProvider loggerProvider)
        {
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Gets the Status value from the headers.
        /// </summary>
        /// <param name="headerDictionary">The header collection.</param>
        /// <returns>A null or an integer value of the status in the headers.</returns>
        protected int? GetStatus(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                return null;
            }

            this.loggerProvider.Debug($"{nameof(this.GetStatus)} Checking for header \"{HeaderNameStatus}\"...");

            if (headerDictionary.ContainsKey(HeaderNameStatus))
            {
                var statusString = headerDictionary[HeaderNameStatus];

                this.loggerProvider.Info(
                    $"{nameof(this.GetStatus)} Header \"{HeaderNameStatus}\" was specified: " +
                    $"\"{statusString}\". Parsing...");

                try
                {
                    var status = (ControlState)Enum.Parse(typeof(ControlState), statusString, true);

                    this.loggerProvider.Info($"{nameof(GetStatus)} Supplied Status is {status}.");

                    return (int)status;
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    this.loggerProvider.Error(
                        $"{nameof(this.GetStatus)} Unable to parse the value of " +
                        $"\"{HeaderNameRunIdentifier}\" " +
                        $"(\"{statusString}\") as a value in the range {ControlState.Start}..{ControlState.Attachments} or {ControlState.Delivered}.");
                }
            }
            else
            {
                this.loggerProvider.Error(
                    $"{nameof(this.GetStatus)} A valid status was not supplied. The " +
                    $"status must be " +
                    $"specified as a valid {nameof(ControlState)} value in the range {ControlState.Start}..{ControlState.Attachments} or {ControlState.Delivered}.");
            }

            return null;
        }

        /// <summary>
        /// Gets the run identifier date and time from the headers.
        /// </summary>
        /// <param name="headerDictionary">
        /// The headers collection.
        /// </param>
        /// <returns>
        /// Null or the date and time.
        /// </returns>
        protected DateTime? GetRunIdentifier(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                return null;
            }

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

        /// <summary>
        /// Gets the run message from the headers if any.
        /// </summary>
        /// <param name="headerDictionary">
        /// The headers collection.
        /// </param>
        /// <returns>
        /// Null or the message string.
        /// </returns>
        protected string GetMessage(IHeaderDictionary headerDictionary)
        {
            if (headerDictionary == null)
            {
                return null;
            }

            string message = null;

            this.loggerProvider.Debug($"Checking for header \"{HeaderNameMessage}\"...");

            if (headerDictionary.ContainsKey(HeaderNameMessage))
            {
                message = headerDictionary[HeaderNameMessage];

                this.loggerProvider.Info(
                    $"Header \"{HeaderNameRunIdentifier}\" was specified: " +
                    $"\"{message}\". Parsing...");
            }

            return message;
        }
    }
}