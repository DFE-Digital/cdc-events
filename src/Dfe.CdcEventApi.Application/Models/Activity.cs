namespace Dfe.CdcEventApi.Application.Models
{
    using System.Collections.Generic;
    using Dfe.CdcEventApi.Application.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an individual <c>activities</c> entity.
    /// </summary>
    [DataHandler("Create_Raw_Activity", "Post")]
    public class Activity : ModelsBase
    {
        /// <summary>
        /// Gets or sets the <c>Fields</c> property.
        /// </summary>
        [DataHandler("Create_Raw_Activity_Field", "Post")]
        [JsonProperty("Fields")]
        public IEnumerable<CustomField> Fields
        {
            get;
            set;
        }
    }
}