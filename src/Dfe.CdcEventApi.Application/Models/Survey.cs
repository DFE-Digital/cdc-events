﻿namespace Dfe.CdcEventApi.Application.Models
{
    using System.Collections.Generic;
    using Dfe.CdcEventApi.Application.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an individual <c>surveys</c> entity.
    /// </summary>
    [DataHandler("Create_Raw_Survey")]
    public class Survey : ModelsBase
    {
        /// <summary>
        /// Gets or sets the <c>Evidence</c> property.
        /// </summary>
        [DataHandler("Create_Raw_Survey_Evidence")]
        [JsonProperty("Evidence")]
        public IEnumerable<CustomField> Evidence
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <c>Fields</c> property.
        /// </summary>
        [DataHandler("Create_Raw_Survey_Field")]
        [JsonProperty("Fields")]
        public IEnumerable<CustomField> Fields
        {
            get;
            set;
        }
    }
}