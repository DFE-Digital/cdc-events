namespace Dfe.CdcEventApi.Application.Models
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Meridian.MeaningfulToString;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Abstract base class for all models in the
    /// <see cref="Dfe.CdcEventApi.Application.Models" /> namespace.
    /// </summary>
    public abstract class ModelsBase : MeaningfulBase
    {
        /// <summary>
        /// Gets or sets all fields, not mapped by descendant types, as
        /// key-value pairs.
        /// </summary>
        [SuppressMessage(
            "Usage",
            "CA2227",
            Justification = "Class is a DTO.")]
        [JsonExtensionData]
        public IDictionary<string, JToken> Data
        {
            get;
            set;
        }
    }
}