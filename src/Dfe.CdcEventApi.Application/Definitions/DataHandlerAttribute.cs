namespace Dfe.CdcEventApi.Application.Definitions
{
    using System;

    /// <summary>
    /// Custom attribute used to apply meta data to the entity models,
    /// describing the run-time data handler for that model.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class | AttributeTargets.Property,
        AllowMultiple = false)]
    public class DataHandlerAttribute : Attribute
    {
        /// <summary>
        /// Initialises a new instance of the
        /// <see cref="DataHandlerAttribute" /> class.
        /// </summary>
        /// <param name="identifier">
        /// The data handler identifier.
        /// </param>
        public DataHandlerAttribute(string identifier)
        {
            this.Identifier = identifier;
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