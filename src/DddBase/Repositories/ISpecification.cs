using System;
using System.Linq.Expressions;

namespace DddBase.Repositories
{
    internal interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }

        Expression<Func<T, object>> OrderBy { get; }
    }
}
