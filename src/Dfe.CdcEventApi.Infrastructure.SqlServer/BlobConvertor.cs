namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System.Collections.Generic;
    using System.Linq;
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
        /// Converts a collection of <see cref="Blob"/> to a collection of<see cref="StorageBlob"/>.
        /// </summary>
        /// <param name="blobs">The collection of <see cref="Blob"/>.</param>
        /// <returns>An instance of <see cref="Task"/> wrapping a collection of <see cref="StorageBlob"/>.</returns>
        public Task<IEnumerable<StorageBlob>> Convert(IEnumerable<Blob> blobs)
        {
            var results = new List<StorageBlob>();

            if (blobs == null)
            {
                return Task.FromResult(results.AsEnumerable());
            }

            foreach (var blob in blobs)
            {
                var storageBlob = new StorageBlob
                {
                    BlobKey = blob.BlobKey,
                    MimeType = blob.MimeType,
                    ParentObjectId = blob.ParentObjectId,
                    ParentObjectType = blob.ParentObjectType,
                    RunIdentifier = blob.RunIdentifier,
                };

                if (blob.BlobData != null)
                {
                    if (blob.BlobData is string)
                    {
                        storageBlob.BlobData = System.Text.Encoding.UTF8.GetBytes(blob.BlobData);
                    }

                    if (blob.BlobData is object)
                    {
                        string stringData = JsonConvert.SerializeObject(blob.BlobData);

                        // is this the best way to identify an azure content object
                        if (
                            stringData.Contains("\"$content-type\":")
                            &&
                            stringData.Contains("\"$content\":"))
                        {
                            var azureObject = JsonConvert.DeserializeObject<AzureBinaryContent>(
                                stringData);
                            storageBlob.BlobData = System.Convert.FromBase64String(
                                azureObject.Content);
                        }
                        else
                        {
                            storageBlob.BlobData = System.Text.Encoding.UTF8.GetBytes(stringData);
                        }
                    }
                }

                results.Add(storageBlob);
            }

            return Task.FromResult(results.AsEnumerable());
        }
    }
}
