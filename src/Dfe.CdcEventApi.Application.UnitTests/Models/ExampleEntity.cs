namespace Dfe.CdcEventApi.Application.UnitTests.Models
{
    using System.Collections.Generic;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Application.Models;
    using Newtonsoft.Json;

    [DataHandler("ExampleDataHandler")]
    public class ExampleEntity : ModelsBase
    {
        [DataHandler("ExampleSubEntityDataHandler")]
        [JsonProperty("additional")]
        public IEnumerable<ExampleSubEntity> AdditionalData
        {
            get;
            set;
        }
    }
}
