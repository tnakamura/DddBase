using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DddBase.Repositories
{
    // TODO: API Design
    internal interface IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : IAggregateRoot<TKey>
    {
        Task<IEnumerable<TAggregateRoot>> ResolveAllAsync(CancellationToken cancellationToken = default);

        Task<TAggregateRoot> ResolveAsync(TKey id, CancellationToken cancellationToken = default);

        Task StoreAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);

        Task DeleteAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default);

        Task<IEnumerable<TAggregateRoot>> FilterAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken = default);

        Task<int> CountAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken = default);
    }
}
