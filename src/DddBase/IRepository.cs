using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;

namespace DddBase
{
    public interface IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        Task<TEntity> ResolveAsync(TKey key);

        Task StoreAsync(TEntity entity);
    }
}
