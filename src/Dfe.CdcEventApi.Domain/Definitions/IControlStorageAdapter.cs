namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Describes the operations of the entity storage adapter.
    /// </summary>
    public interface IControlStorageAdapter
    {
        /// <summary>
        /// Starts the load by creating and returning a new <see cref="Control"/> model.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an instance of <see cref="Control"/> of the run start.
        /// </returns>
        Task<IEnumerable<Control>> CreateAsync(DateTime runIdentifier);

        /// <summary>
        /// Updates the status of a <see cref="Control"/>.
        /// </summary>
        /// <param name="runIdentifier">
        /// The identifier of the <see cref="Control"/>.
        /// </param>
        /// <param name="status">
        /// The valof the new status.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        Task UpdateStatusAsync(DateTime runIdentifier, int status);

        /// <summary>
        /// Gets the <see cref="Control"/> for the specified date and time.
        /// </summary>
        /// <param name="runIdentifier">
        /// The run identifier start date time value.
        /// </param>
        /// <returns>
        /// An <see cref="Task"/> wrapping an collection of <see cref="Control"/> of the run.
        /// </returns>
        Task<Control> GetAsync(DateTime runIdentifier);

        /// <summary>
        /// Updates a <see cref="Control"/> of the specified date and time.
        /// </summary>
        /// <param name="item">
        /// The new version of the <see cref="Control"/> item.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> instance.
        /// </returns>
        Task UpdateAsync(Control item);

        /// <summary>
        /// Gets the loaded row count for the run.
        /// </summary>
        /// <param name="runIdentifier">
        /// The date and time.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> wrapping an <see cref="int"/>.
        /// </returns>
        Task<int> GetCountAsync(DateTime runIdentifier);
    }
}