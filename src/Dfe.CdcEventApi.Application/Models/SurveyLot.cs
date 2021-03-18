namespace Dfe.CdcEventApi.Application.Models
{
    using System.Collections.Generic;
    using Dfe.CdcEventApi.Application.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an individual <c>surveylots</c> entity.
    /// </summary>
    [DataHandler("Create_Raw_SurveyLot")]
    public class SurveyLot : ModelsBase
    {
        /// <summary>
        /// Gets or sets the <c>Fields</c> property.
        /// </summary>
        [DataHandler("Create_Raw_SurveyLot_Field")]
        [JsonProperty("Fields")]
        public IEnumerable<CustomField> Fields
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <c>SurveyLotSiteInfos</c> property.
        /// </summary>
        [DataHandler("Create_Raw_SurveyLot_SiteInfo")]
        [JsonProperty("SurveyLotSiteInfos")]
        public IEnumerable<SiteInfo> SiteInfos
        {
            get;
            set;
        }
    }
}