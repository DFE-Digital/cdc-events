namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Defines the mapping of <see cref="Blob"/> to <see cref="StorageBlob"/> instances.
    /// </summary>
    public interface IBlobConvertor
    {
        /// <summary>
        /// Converts collections of <see cref="Blob"/> to collections of <see cref="StorageBlob"/>.
        /// </summary>
        /// <param name="blobs">the collection of <see cref="Blob"/>.</param>
        /// <returns>An instance of <see cref="IEnumerable{StorageBlob}"/>.</returns>
        Task<IEnumerable<StorageBlob>> Convert(IEnumerable<Blob> blobs);
    }
}
