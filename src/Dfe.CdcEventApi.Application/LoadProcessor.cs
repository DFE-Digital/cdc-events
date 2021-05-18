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
    /// .
    /// </summary>
    public class LoadProcessor : ILoadProcessor
    {
        private ILoadStorageAdapter loadStorageAdapter;
        private ILoggerProvider loggerProvider;

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
            var loads = await this.loadStorageAdapter.CreateLoadAsync(runIdentifier)
                                .ConfigureAwait(false);
            return loads;
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <param name="cancellationToken">...</param>
        /// <returns>....</returns>
        public async Task<Load> GetLoadAsync(
                DateTime runIdentifier,
                CancellationToken cancellationToken)
        {
            var load = await this.loadStorageAdapter.GetLoadAsync(runIdentifier)
                    .ConfigureAwait(false);
            return load;
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
        /// ....
        /// </returns>
        public async Task<IEnumerable<LoadNotification>> GetLoadNotifications(
            short status,
            CancellationToken cancellationToken)
        {
            return await this.loadStorageAdapter
                                 .GetLoadNotificationsForStatus(status)
                                 .ConfigureAwait(false);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="status">..</param>
        /// <param name="cancellationToken">...</param>
        /// <returns>....</returns>
        public async Task<LoadNotificationTemplate> GetLoadTemplateForStatus(
            short status,
            CancellationToken cancellationToken)
        {
            return await this.loadStorageAdapter
                                .GetLoadTemplateForStatus(status)
                                .ConfigureAwait(false);
        }

        /// <summary>
        /// .
        /// </summary>
        /// <param name="item">..</param>
        /// <param name="cancellationToken">...</param>
        /// <returns>....</returns>
        public async Task UpdateLoadAsync(
            Load item,
            CancellationToken cancellationToken)
        {
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
            await this.loadStorageAdapter
                        .UpdateLoadStatusAsync(runIdentifier, status)
                        .ConfigureAwait(false);
        }
    }
}