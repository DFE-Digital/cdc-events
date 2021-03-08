namespace Dfe.CdcEventApi.Infrastructure.SqlServer
{
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;
    using Dfe.CdcEventApi.Domain.Definitions;

    /// <summary>
    /// Implements <see cref="IEntityStorageAdapter" />.
    /// </summary>
    public class EntityStorageAdapter : IEntityStorageAdapter
    {
        /// <inheritdoc />
        public Task StoreEntitiesAsync(
            string dataHandlerIdentifier,
            DataTable dataTable,
            CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}