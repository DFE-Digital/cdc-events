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
    public class LoadHandlerAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="LoadHandlerAttribute" /> class.
        /// </summary>
        /// <param name="verb">
        /// The load handler request verb.
        /// </param>
        /// <param name="identifier">
        /// The load handler identifier.
        /// </param>
        public LoadHandlerAttribute(string verb, string identifier)
        {
            this.Verb = verb;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets the load handler request verb.
        /// </summary>
        public string Verb
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the data handler identifier.
        /// </summary>
        public string Identifier
        {
            get;
            private set;
        }
    }
}