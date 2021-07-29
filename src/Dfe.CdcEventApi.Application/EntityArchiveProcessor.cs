namespace Dfe.CdcEventApi.Application
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;

    /// <summary>
    /// Implements <see cref="IEntityArchiveProcessor"/>.
    /// </summary>
    public class EntityArchiveProcessor : IEntityArchiveProcessor
    {
        private readonly IEntityArchiveAdapter entityArchiveAdapter;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityArchiveProcessor"/> class.
        /// </summary>
        /// <param name="entityArchiveAdapter">An instance of <see cref="IEntityArchiveAdapter"/>.</param>
        /// <param name="loggerProvider">An instance of <see cref="ILoggerProvider"/>.</param>
        public EntityArchiveProcessor(IEntityArchiveAdapter entityArchiveAdapter, ILoggerProvider loggerProvider)
        {
            this.entityArchiveAdapter = entityArchiveAdapter;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Processes teh incoming JSON data and passes it to the adapter for storage.
        /// </summary>
        /// <param name="entityType">The entity type name.</param>
        /// <param name="runIdentifier">the session date and time stamp.</param>
        /// <param name="data">The JSON data as a string.</param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/>.</param>
        /// <returns>An instance of <see cref="Task"/>.</returns>
        public async Task CreateAsync(string entityType, DateTime runIdentifier, string data, CancellationToken cancellationToken)
        {
            // Generate stored item name
            var itemName = $"{entityType}-{runIdentifier:O}-{DateTime.UtcNow:0}.json";
            this.loggerProvider.Debug($"Storing entity data for entity {entityType} and run identifier {runIdentifier:O} under item name {itemName}");

            // store the item under this name.
            await this.entityArchiveAdapter.StoreAsync(itemName, data, cancellationToken).ConfigureAwait(false);
        }
    }
}