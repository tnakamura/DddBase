using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DddBase.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DddBase.EntityFrameworkCore
{
    internal abstract class EntityRepository<TAggregate, TKey> : IRepository<TAggregate, TKey>
        where TAggregate : class, IAggregateRoot<TKey>
    {
        readonly DbContext dbContext;

        protected EntityRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        DbSet<TAggregate> Entities => dbContext.Set<TAggregate>();

        public Task DeleteAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TAggregate aggregate, CancellationToken cancellationToken = default(CancellationToken))
        {
            Entities.Remove(aggregate);
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task<IEnumerable<TAggregate>> ResolveAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TAggregate> ResolveAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Entities.FindAsync(id, cancellationToken);
        }

        public Task StoreAsync(TAggregate aggregate, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
