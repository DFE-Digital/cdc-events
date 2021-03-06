﻿namespace Dfe.CdcEventApi.Application.Models
{
    using System.Collections.Generic;
    using Dfe.CdcEventApi.Application.Definitions;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents an individual <c>protfolios</c> entity.
    /// </summary>
    [DataHandler("Create_Raw_Portfolio")]
    public class Portfolio : ModelsBase
    {
        /// <summary>
        /// Gets or sets the <c>Fields</c> property.
        /// </summary>
        [DataHandler("Create_Raw_Portfolio_Field")]
        [JsonProperty("Fields")]
        public IEnumerable<CustomField> Fields
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <c>PortfolioSiteInfos</c> property.
        /// </summary>
        [DataHandler("Create_Raw_Portfolio_SiteInfo")]
        [JsonProperty("PortfolioSiteInfos")]
        public IEnumerable<SiteInfo> SiteInfos
        {
            get;
            set;
        }
    }
}