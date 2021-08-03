namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Implements <see cref="IBlobConvertor"/>.
    /// </summary>
    public class BlobConvertor : IBlobConvertor
    {
        /// <summary>
        /// Converts a collection of <see cref="AttachmentResponse"/> data bytes.
        /// </summary>
        /// <param name="blob">The <see cref="AttachmentResponse"/>.</param>
        /// <returns>
        /// An instance of <see cref="Task"/> wrapping a collection of <see cref="byte"/> according to the blob data object type.
        /// </returns>
        public byte[] Convert(AttachmentResponse blob)
        {
            if (blob == null)
            {
                return null as byte[];
            }

            if (blob.Data != null)
            {
                if (blob.Data is string)
                {
                    return System.Text.Encoding.UTF8.GetBytes(blob.Data as string);
                }

                if (blob.Data is object)
                {
                    string stringData = JsonConvert.SerializeObject(blob.Data);

                    // is this the best way to identify an azure content object
                    if (
                        stringData.Contains("\"$content-type\":")
                        &&
                        stringData.Contains("\"$content\":"))
                    {
                        var azureObject = JsonConvert.DeserializeObject<AzureBinaryContent>(
                            stringData);
                        return System.Convert.FromBase64String(azureObject.Content);
                    }
                    else
                    {
                        return System.Text.Encoding.UTF8.GetBytes(stringData);
                    }
                }
            }

            return null as byte[];
        }
    }
}