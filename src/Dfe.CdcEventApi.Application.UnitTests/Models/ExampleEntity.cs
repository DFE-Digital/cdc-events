namespace Dfe.CdcEventApi.Application.UnitTests.Models
{
    using System.Collections.Generic;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Models;
    using Newtonsoft.Json;

    [DataHandler("ExampleDataHandler", "Post")]
    [DataHandler("ExampleDataHandler", "Get")]
    public class ExampleEntity : ModelsBase
    {
        [DataHandler("ExampleSubEntityDataHandler","Post")]
        [DataHandler("ExampleSubEntityDataHandler", "Get")]
        [JsonProperty("additional")]
        public IEnumerable<ExampleSubEntity> AdditionalData
        {
            get;
            set;
        }
    }
}
