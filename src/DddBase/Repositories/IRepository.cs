using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DddBase.Repositories
{
    internal interface IRepository<TAggregate, TKey>
        where TAggregate : IAggregate<TKey>
    {
        Task<IEnumerable<TAggregate>> ResolveAllAsync(CancellationToken cancellationToken = default);

        Task<TAggregate> ResolveAsync(TKey id, CancellationToken cancellationToken = default);

        Task StoreAsync(TAggregate entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(TAggregate entity, CancellationToken cancellationToken = default);

        Task DeleteAllAsync(CancellationToken cancellationToken = default);
    }
}
