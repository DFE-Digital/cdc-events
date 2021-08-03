namespace Dfe.CdcEventApi.Domain.Definitions
{
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Defines the mapping of <see cref="AttachmentResponse"/> object data property to a byte array.
    /// </summary>
    public interface IBlobConvertor
    {
        /// <summary>
        /// Converts <see cref="AttachmentResponse"/> data content to an array of bytes.
        /// </summary>
        /// <param name="blob">The <see cref="AttachmentResponse"/>.</param>
        /// <returns>An array of <see cref="byte"/>.</returns>
        byte[] Convert(AttachmentResponse blob);
    }
}