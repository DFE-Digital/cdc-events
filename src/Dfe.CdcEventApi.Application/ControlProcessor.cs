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
    /// Implements <see cref="IControlProcessor" />.
    /// </summary>
    public class ControlProcessor : IControlProcessor
    {
        private readonly IControlStorageAdapter loadStorageAdapter;
        private readonly ILoggerProvider loggerProvider;

        /// <summary>
        /// Initialises a new instance of the <see cref="ControlProcessor"/> class.
        /// </summary>
        /// <param name="loadStorageAdapter">
        /// An injected instance of <see cref="IControlStorageAdapter"/>.
        /// </param>
        /// <param name="loggerProvider">
        /// An injected instance of <see cref="ILoggerProvider"/>.
        /// </param>
        public ControlProcessor(
            IControlStorageAdapter loadStorageAdapter,
            ILoggerProvider loggerProvider)
        {
            this.loadStorageAdapter = loadStorageAdapter;
            this.loggerProvider = loggerProvider;
        }

        /// <summary>
        /// Creates a <see cref="Control"/> entity at the start or a load event.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier date and time value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asnychronous processing cancellation token.
        /// </param>
        /// <returns>
        /// A instance of a <see cref="Task"/> bearing an instance of the created <see cref="Control"/>.
        /// </returns>
        public async Task<IEnumerable<Control>> CreateAsync(
            DateTime runIdentifier,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Creating a new Load...");
            var loads = await this.loadStorageAdapter.CreateAsync(runIdentifier)
                                .ConfigureAwait(false);
            return loads;
        }

        /// <summary>
        /// Gets an instance of <see cref="Control"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The instance identifier value.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping the <see cref="Control"/> instance.
        /// </returns>
        public async Task<Control> GetAsync(
                DateTime runIdentifier,
                CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Getting the Load data...");
            var load = await this.loadStorageAdapter.GetAsync(runIdentifier)
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
        public async Task<int> GetCountAsync(DateTime runIdentifier, CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Getting the load row count...");
            return await this.loadStorageAdapter
                                 .GetCountAsync(runIdentifier)
                                 .ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads a complete instance of <see cref="Control"/>.
        /// </summary>
        /// <param name="item">
        /// The changed <see cref="Control"/> instance.
        /// </param>
        /// <param name="cancellationToken">
        /// The asynchronous <see cref="CancellationToken"/>.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/>.
        /// </returns>
        public async Task UpdateAsync(
            Control item,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Updating the load...");
            await this.loadStorageAdapter
                        .UpdateAsync(item)
                        .ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the <see cref="Control"/> status value.
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
        public async Task UpdateStatusAsync(
            DateTime runIdentifier,
            int status,
            CancellationToken cancellationToken)
        {
            this.loggerProvider.Debug($"Updating the load status...");
            await this.loadStorageAdapter
                        .UpdateStatusAsync(runIdentifier, status)
                        .ConfigureAwait(false);
        }
    }
}