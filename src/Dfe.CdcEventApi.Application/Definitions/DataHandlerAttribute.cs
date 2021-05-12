namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;

    /// <summary>
    /// Custom attribute used to apply meta data to the entity models,
    /// describing the run-time data handler for that model.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Property,
        AllowMultiple = true)]
    public class DataHandlerAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="DataHandlerAttribute" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The data handler identifier.
        /// </param>
        /// <param name="verb">
        /// The data handler verb.
        /// </param>
        public DataHandlerAttribute(string identifier, string verb)
        {
            this.Identifier = identifier;
            this.Verb = verb;
        }

        /// <summary>
        /// Gets the data handler identifier.
        /// </summary>
        public string Identifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the data handler verb.
        /// </summary>
        public string Verb
        {
            get;
            private set;
        }
    }
}