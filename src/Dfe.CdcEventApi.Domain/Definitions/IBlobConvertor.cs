namespace Dfe.CdcEventApi.Domain.Definitions
{
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Defines the mapping of <see cref="Blob"/> object data property to a byte array.
    /// </summary>
    public interface IBlobConvertor
    {
        /// <summary>
        /// Converts <see cref="Blob"/> data content to a string.
        /// </summary>
        /// <param name="blob">The <see cref="Blob"/>.</param>
        /// <returns>An array of <see cref="byte"/>.</returns>
        byte[] Convert(Blob blob);
    }
}