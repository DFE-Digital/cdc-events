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
    /// Implements <see cref="ILoadProcessor" />.
    /// </summary>
    public class LoadProcessor : ILoadProcessor
    {
        private readonly ILoadStorageAdapter loadStorageAdapter;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="LoadProcessor"/> class.
        /// </summary>
        /// <param name="loadStorageAdapter">
        /// An injected instance of <see cref="ILoadStorageAdapter"/>.
        /// </param>
        /// <param name="loggerProvider">
        /// An injected instance of <see cref="ILoggerProvider"/>.
        /// </param>
        public LoadProcessor(
            ILoadStorageAdapter loadStorageAdapter,
            ILoggerProvider loggerProvider)
        {
            this.loadStorageAdapter = loadStorageAdapter;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Creates a <see cref="Load"/> entity at the start or a load event.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier date and time value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous processing cancellation token.
        /// </param>
        /// <returns>
        /// A instance of a <see cref="Task"/> bearing an instance of the created <see cref="Load"/>.
        /// </returns>
        public async Task<IEnumerable<Load>> CreateLoadAsync(
            DateTime runIdentifier,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Creating a new Load...");
            var loads = await this.loadStorageAdapter.CreateLoadAsync(runIdentifier)
                                .ConfigureAwait(false);
            return loads;
        }

        /// <summary>
        /// Executes the load extraction process.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier date and time value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous processing cancellation token.
        /// </param>
        /// <returns>
        /// A instance of a <see cref="Task"/>.
        /// </returns>
        public async Task ExecuteExtract(DateTime runIdentifier, CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Extracting loaded data, this may take a while...");
            await this.loadStorageAdapter.ExecuteExtract(runIdentifier)
                                .ConfigureAwait(false);
        }

        /// <summary>
        /// Perform transform of etl model into condition model.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier date and time value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous processing cancellation token.
        /// </param>
        /// <returns>
        /// A instance of a <see cref="Task"/>.
        /// </returns>
        public async Task ExecuteTransform(DateTime runIdentifier, CancellationToken cancellationToken)
        {

            this.loggerProvider.Debug($"Transforming loaded data, this may take a short while...");
            await this.loadStorageAdapter.ExecuteTransform(runIdentifier)
                                .ConfigureAwait(false);
        }

        /// <summary>
        /// Get all <see cref="Attachment"/> instances deriving from the specfied <see cref="Load "/> date and time.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the collection of <see cref="Attachment"/>.
        /// </returns>
        public async Task<IEnumerable<Attachment>> GetAttachments(DateTime runIdentifier, CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Getting attachments to process...");
            var attachments = await this.loadStorageAdapter.GetAttachments(runIdentifier).ConfigureAwait(false);
            return attachments;
        }

        /// <summary>
        /// Gets an instance of <see cref="Load"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The instance identifier value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="Load"/> instance.
        /// </returns>
        public async Task<Load> GetLoadAsync(
                DateTime runIdentifier,
                CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Getting the Load data...");
            var load = await this.loadStorageAdapter.GetLoadAsync(runIdentifier)
                    .ConfigureAwait(false);
            return load;
        }

        /// <summary>
        /// Gets the loaded row count for the run.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping an <see cref="int"/>.
        /// </returns>
        public async Task<int> GetLoadCountAsync(DateTime runIdentifier, CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Getting the load row count...");
            return await this.loadStorageAdapter
                                 .GetLoadCountAsync(runIdentifier)
                                 .ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves all the required <see cref="LoadNotification"/> entities for the specified run status value.
        /// </summary>
        /// <param name="status">
        /// The run status value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous processing cancellation token.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping a collection of <see cref="LoadNotification"/>.
        /// </returns>
        public async Task<IEnumerable<LoadNotification>> GetLoadNotifications(
            short status,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Getting the load notifications to process...");
            return await this.loadStorageAdapter
                                 .GetLoadNotificationsForStatus(status)
                                 .ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the <see cref="LoadNotificationTemplate"/> for the specified status value.
        /// </summary>
        /// <param name="status">
        /// The status value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="LoadNotificationTemplate"/>.
        /// </returns>
        public async Task<LoadNotificationTemplate> GetLoadTemplateForStatus(
            short status,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Getting load status template...");
            return await this.loadStorageAdapter
                                .GetLoadTemplateForStatus(status)
                                .ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a complete instance of <see cref="Load"/>.
        /// </summary>
        /// <param name="item">
        /// The changed <see cref="Load"/> instance.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        public async Task UpdateLoadAsync(
            Load item,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Updating the load...");
            await this.loadStorageAdapter
                        .UpdateLoadAsync(item)
                        .ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the <see cref="Load"/> status value.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier date and time value.
        /// </param>
        /// <param name="status">
        /// The status value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous processing cancellation token.
        /// </param>
        /// <returns>
        /// A base instance of <see cref="Task"/>.
        /// </returns>
        public async Task UpdateLoadStatusAsync(
            DateTime runIdentifier,
            short status,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Updating the load status...");
            await this.loadStorageAdapter
                        .UpdateLoadStatusAsync(runIdentifier, status)
                        .ConfigureAwait(false);
        }
    }
}