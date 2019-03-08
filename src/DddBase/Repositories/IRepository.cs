using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DddBase.Repositories
{
    internal interface IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        Task<IEnumerable<TEntity>> ResolveAllAsync(CancellationToken cancellationToken = default);

        Task<TEntity> ResolveAsync(TKey id, CancellationToken cancellationToken = default);

        Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAllAsync(CancellationToken cancellationToken = default);
    }
}
