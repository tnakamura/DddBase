using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;

namespace DddBase.Repositories
{
    internal class InMemoryRepository<TAggregate, TKey> : IRepository<TAggregate, TKey>
        where TAggregate : IAggregate<TKey>
    {
        readonly ConcurrentDictionary<TKey, TAggregate> dictionary;

        public InMemoryRepository()
        {
            dictionary = new ConcurrentDictionary<TKey, TAggregate>();
        }

        public Task<TAggregate> ResolveAsync(TKey id, CancellationToken cancellationToken = default)
        {
            if (dictionary.TryGetValue(id, out var value))
            {
                return Task.FromResult(value);
            }
            else
            {
                return Task.FromResult(default(TAggregate));
            }
        }

        public Task StoreAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
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

        public Task<IEnumerable<TAggregate>> ResolveAllAsync(
            Expression<Func<TAggregate, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult(dictionary.Values.Where(predicate.Compile()));
        }

        public Task<IEnumerable<TAggregate>> ResolveAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<TAggregate>>(dictionary.Values);
        }

        public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default)
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
            dictionary.TryRemove(aggregate.Id, out _);
            return Task.CompletedTask;
        }
    }
}
