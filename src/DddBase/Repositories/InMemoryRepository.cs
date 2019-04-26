using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace DddBase.Repositories
{
    internal class InMemoryRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : IAggregateRoot<TKey>
    {
        readonly ConcurrentDictionary<TKey, TAggregateRoot> dictionary;

        public InMemoryRepository()
        {
            dictionary = new ConcurrentDictionary<TKey, TAggregateRoot>();
        }

        public Task<TAggregateRoot> ResolveAsync(TKey id, CancellationToken cancellationToken = default)
        {
            if (dictionary.TryGetValue(id, out var value))
            {
                return Task.FromResult(value);
            }
            else
            {
                return Task.FromResult(default(TAggregateRoot));
            }
        }

        public Task StoreAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
            dictionary[aggregate.Id] = aggregate;
            return Task.CompletedTask;
        }

        public Task DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            dictionary.Clear();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TAggregateRoot>> ResolveAllAsync(
            Expression<Func<TAggregateRoot, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(dictionary.Values.Where(predicate.Compile()));
        }

        public Task<IEnumerable<TAggregateRoot>> ResolveAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<TAggregateRoot>>(dictionary.Values);
        }

        public Task DeleteAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
            dictionary.TryRemove(aggregate.Id, out _);
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TAggregateRoot>> FilterAsync(ISpecification<TAggregateRoot> specification, CancellationToken cancellationToken = default)
        {
            IEnumerable<TAggregateRoot> items = dictionary.Values;
            if (specification.Criteria != null)
            {
                items = items.Where(specification.Criteria.Compile());
            }
            if (specification.OrderBy != null)
            {
                items = items.OrderBy(specification.OrderBy.Compile());
            }
            return Task.FromResult(items);
        }
    }
}
