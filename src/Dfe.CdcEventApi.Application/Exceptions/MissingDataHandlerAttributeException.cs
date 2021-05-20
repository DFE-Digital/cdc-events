namespace Dfe.CdcEventApi.Application.Exceptions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Dfe.CdcEventApi.Application.Definitions;

    /// <summary>
    /// Exception thrown when the <see cref="DataHandlerAttribute" /> is not
    /// present on a given entity model, when attempting to process it.
    /// </summary>
    [SuppressMessage(
        "Microsoft.Design",
        "CA1032",
        Justification = "Not a public library.")]
    public class MissingDataHandlerAttributeException : MissingMemberException
    {
        private new const string Message =
            "The type, \"{0}\", is missing the DataHandlerAttribute.";

        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="MissingDataHandlerAttributeException" /> class.
        /// </summary>
        /// <param name="type">
        /// The type with the missing <see cref="DataHandlerAttribute" />.
        /// </param>
        public MissingDataHandlerAttributeException(Type type)
            : base(BuildExceptionMessage(type))
        {
            // Nothing for now.
        }

        private static string BuildExceptionMessage(Type type)
        {
            string typeStr = type.Name;

            string toReturn = string.Format(
                                CultureInfo.InvariantCulture,
                                Message,
                                typeStr);
            return toReturn;
        }
    }
}