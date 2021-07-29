namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Implements <see cref="IEntityArchiveAdapter"/>.
    /// </summary>
    public class EntityArchiveAdapter : IEntityArchiveAdapter
    {
        private readonly ILoggerProvider loggerProvider;
        private readonly string connectionString;
        private readonly string containerName;

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityArchiveAdapter"/> class.
        /// </summary>
        /// <param name="attachmentSettingsProvider">An instance of <see cref="IAttachmentSettingsProvider"/>.</param>
        /// <param name="loggerProvider">An instance of <see cref="ILoggerProvider"/>.</param>
        public EntityArchiveAdapter(IAttachmentSettingsProvider attachmentSettingsProvider, ILoggerProvider loggerProvider)
        {
            if (attachmentSettingsProvider == null)
            {
                throw new ArgumentNullException(nameof(attachmentSettingsProvider));
            }

            this.connectionString = attachmentSettingsProvider.AttachmentStorageConnectionString;
            this.containerName = attachmentSettingsProvider.ArchiveContainerName;
            this.loggerProvider = loggerProvider;
        }

        /// <inheritdoc/>
        public async Task StoreAsync(string itemName, string itemData, CancellationToken cancellationToken)
        {
            this.loggerProvider.Info($"Storing {itemName}.");

            // Get a reference to a container named "sample-container" and then create it
            this.loggerProvider.Info($"Getting the storage account client.");
            BlobContainerClient container = new BlobContainerClient(this.connectionString, this.containerName);

            if (await container.ExistsAsync(cancellationToken).ConfigureAwait(false) == false)
            {
                this.loggerProvider.Debug($"Creating the container {this.containerName}");
                await container.CreateAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            this.loggerProvider.Debug($"Geting blob client for {itemName}");
            BlobClient blob = container.GetBlobClient(itemName);

            this.loggerProvider.Debug($"Storing {itemName} data.");
            blob.Upload(new BinaryData(itemData), cancellationToken);
        }
    }
}