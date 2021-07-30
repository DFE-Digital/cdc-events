namespace Dfe.CdcEventApi.Application
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Implements <see cref="IAttachmentProcessor"/>.
    /// </summary>
    public class AttachmentProcessor : IAttachmentProcessor
    {
        private readonly IAttachmentStorageAdapter attachmentStorageAdapter;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="AttachmentProcessor"/> class.
        /// </summary>
        /// <param name="attachmentStorageAdapter">
        /// An instance of type <see cref="IAttachmentStorageAdapter" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public AttachmentProcessor(
           IAttachmentStorageAdapter attachmentStorageAdapter,
           ILoggerProvider loggerProvider)
        {
            this.attachmentStorageAdapter = attachmentStorageAdapter;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Retrieve a list of required attachments.
        /// </summary>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>A collection of <see cref="AttachmentRequest"/>.</returns>
        public async Task<IEnumerable<AttachmentRequest>> GetAsync(CancellationToken cancellationToken)
        {
            this.loggerProvider.Info($"{nameof(AttachmentProcessor)} Retrieving attachments list.");

            return await this.attachmentStorageAdapter.GetAsync()
                                             .ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a collection of <see cref="AttachmentResponse"/> records.
        /// </summary>
        /// <param name="runIdentifier">The run identifier date and time.</param>
        /// <param name="models">The models to create.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        public async Task PostAsync(
            DateTime runIdentifier,
            IEnumerable<AttachmentResponse> models,
            CancellationToken cancellationToken)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            this.loggerProvider.Info($"{nameof(AttachmentProcessor)} Processing blobs.");

            await this.attachmentStorageAdapter.CreateAsync(
                                            runIdentifier,
                                            models)
                                            .ConfigureAwait(false);
        }
    }
}