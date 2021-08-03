namespace Dfe.CdcEventApi.Domain.Models
{
    using Dfe.CdcEventApi.Domain.Definitions;

    /// <summary>
    /// Defines the properties for the API attachment response.
    /// </summary>
    public interface IAttachmentResponse : IAttachmentRequest
    {
        /// <summary>
        /// Gets or sets the recieved attachment source blob data.
        /// Due to peculiarities of the response of the HTTP call to get the data into the Azure Logic app, this is of variable type that is then handled by an instance of <see cref="IBlobConvertor"/>.
        /// </summary>
        object Data { get; set; }
    }
}