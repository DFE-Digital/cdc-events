namespace Dfe.CdcEventApi.Domain.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the Azure logic app binary content body class.
    /// </summary>
    public class AzureBinaryContent
    {
        /// <summary>
        /// Gets or sets the mime type of the data.
        /// </summary>
        [JsonProperty(PropertyName = "$content-type")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content which is a base64 encoded string of binary data.
        /// </summary>
        [JsonProperty(PropertyName = "$content")]
        public string Content { get; set; }
    }
}
