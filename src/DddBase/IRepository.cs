using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DddBase
{
    public interface IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        Task<IEnumerable<TEntity>> FindAllAsync(CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(TKey key, CancellationToken cancellationToken = default);

        Task SaveAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task ClearAsync(CancellationToken cancellationToken = default);
    }
}
