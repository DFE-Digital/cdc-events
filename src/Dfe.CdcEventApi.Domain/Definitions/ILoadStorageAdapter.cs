namespace Dfe.CdcEventApi.Domain.Definitions
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Models;

    /// <summary>
    /// Describes the operations of the entity storage adapter.
    /// </summary>
    public interface ILoadStorageAdapter
    {
        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <returns>...</returns>
        Task<IEnumerable<Load>> CreateLoadAsync(DateTime runIdentifier);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <param name="status">... </param>
        /// <returns>....</returns>
        Task UpdateLoadStatusAsync(DateTime runIdentifier, short status);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="runIdentifier">..</param>
        /// <returns>...</returns>
        Task<Load> GetLoadAsync(DateTime runIdentifier);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="status">..</param>
        /// <returns>...</returns>
        Task<IEnumerable<LoadNotification>> GetLoadNotificationsForStatus(short status);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="item">...</param>
        /// <returns>....</returns>
        Task UpdateLoadAsync(Load item);

        /// <summary>
        /// .
        /// </summary>
        /// <param name="status">..</param>
        /// <returns>...</returns>
        Task<LoadNotificationTemplate> GetLoadTemplateForStatus(short status);
    }
}