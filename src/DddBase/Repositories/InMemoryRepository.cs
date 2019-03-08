using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DddBase.Repositories
{
    internal class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        readonly ConcurrentDictionary<TKey, TEntity> dictionary;

        public InMemoryRepository()
        {
            dictionary = new ConcurrentDictionary<TKey, TEntity>();
        }

        public Task<TEntity> ResolveAsync(TKey key, CancellationToken cancellationToken = default)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                return Task.FromResult(value);
            }
            else
            {
                return Task.FromResult(default(TEntity));
            }
        }

        public Task StoreAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            dictionary[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task ClearAsync(CancellationToken cancellationToken = default)
        {
            dictionary.Clear();
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TEntity>> ResolveAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<TEntity>>(dictionary.Values);
        }

        public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            dictionary.TryRemove(entity.Id, out _);
            return Task.CompletedTask;
        }
    }
}
