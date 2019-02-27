using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DddBase
{
    public sealed class InMemoryRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        readonly ConcurrentDictionary<TKey, TEntity> dictionary;

        public Task<TEntity> ResolveAsync(TKey key)
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

        public Task StoreAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            dictionary[entity.Id] = entity;
            return Task.CompletedTask;
        }
    }
}
