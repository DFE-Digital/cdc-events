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
    /// Implements <see cref="IBlobProcessor"/>.
    /// </summary>
    public class BlobProcessor : IBlobProcessor
    {
        private readonly ILoadStorageAdapter loadStorageAdapter;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="BlobProcessor"/> class.
        /// </summary>
        /// <param name="loadStorageAdapter">
        /// An instance of type <see cref="ILoadStorageAdapter" />.
        /// </param>
        /// <param name="loggerProvider">
        /// An instance of type <see cref="ILoggerProvider" />.
        /// </param>
        public BlobProcessor(
           ILoadStorageAdapter loadStorageAdapter,
           ILoggerProvider loggerProvider)
        {
            this.loadStorageAdapter = loadStorageAdapter;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Creates a collection of <see cref="Blob"/> records.
        /// </summary>
        /// <param name="runIdentifier">The run identifier date and time.</param>
        /// <param name="models">The models to create.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        public async Task CreateBlobsAsync(
            DateTime runIdentifier,
            IEnumerable<Blob> models,
            CancellationToken cancellationToken)
        {
            if (models == null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            this.loggerProvider.Info($"{nameof(BlobProcessor)} Processing blobs.");

            await this.loadStorageAdapter.CreateBlobsAsync(
                runIdentifier,
                models)
                .ConfigureAwait(false);
        }
    }
}