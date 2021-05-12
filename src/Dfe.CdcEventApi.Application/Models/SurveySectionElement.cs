namespace Dfe.CdcEventApi.Application.Models
{
    using System.Collections.Generic;
    using Dfe.CdcEventApi.Application.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an individual <c>surveysectionelements</c> entity.
    /// </summary>
    [DataHandler("Create_Raw_SurveySectionElement", "Post")]
    public class SurveySectionElement : ModelsBase
    {
        /// <summary>
        /// Gets or sets the <c>Evidence</c> property.
        /// </summary>
        [DataHandler("Create_Raw_SurveySectionElement_Evidence", "Post")]
        [JsonProperty("Evidence")]
        public IEnumerable<CustomField> Evidence
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <c>Fields</c> property.
        /// </summary>
        [DataHandler("Create_Raw_SurveySectionElement_Field", "Post")]
        [JsonProperty("Fields")]
        public IEnumerable<CustomField> Fields
        {
            get;
            set;
        }
    }
}