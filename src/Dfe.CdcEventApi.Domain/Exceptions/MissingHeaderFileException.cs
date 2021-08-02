namespace Dfe.CdcEventApi.Domain.Exceptions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Resources;

    /// <summary>
    /// Exception thrown when a data handler is not present.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1032",
        Justification = "Not a public library.")]
    public class MissingHeaderFileException : MissingManifestResourceException
    {
        private new const string Message =
            "The data handler \"{0}\" does not exist.";

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="MissingHeaderFileException"/> class.
        /// </summary>
        /// <param name="identifier">
        /// The data handler identifier.
        /// </param>
        public MissingHeaderFileException(string identifier)
            : base(BuildExceptionMessage(identifier))
        {
            // Nothing for now.
        }

        private static string BuildExceptionMessage(string identifier)
        {
            string toReturn = string.Format(
                CultureInfo.InvariantCulture,
                Message,
                identifier);

            return toReturn;
        }
    }
}