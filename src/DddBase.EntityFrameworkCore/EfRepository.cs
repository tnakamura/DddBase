using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DddBase.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DddBase.EntityFrameworkCore
{
    internal abstract class EfRepository<TAggregateRoot, TKey> : IRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        readonly DbContext dbContext;

        protected EfRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task DeleteAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default(CancellationToken))
        {
            dbContext.Set<TAggregateRoot>().Remove(aggregate);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TAggregateRoot>> ResolveAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Set<TAggregateRoot>()
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TAggregateRoot> ResolveAsync(TKey id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbContext.Set<TAggregateRoot>()
                .FindAsync(id)
                .ConfigureAwait(false);
        }

        public async Task StoreAsync(TAggregateRoot aggregate, CancellationToken cancellationToken = default(CancellationToken))
        {
            dbContext.Set<TAggregateRoot>().Update(aggregate);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TAggregateRoot>> FilterAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Set<TAggregateRoot>().AsQueryable();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            return await query.ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<int> CountAsync(ISpecification<TAggregateRoot> spec, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Set<TAggregateRoot>().AsQueryable();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            return await query.CountAsync().ConfigureAwait(false);
        }
    }
}
