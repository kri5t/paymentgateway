using System;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PaymentGatewayDatabase;

namespace PaymentGatewayCore.Architecture
{
    [UsedImplicitly]
    public class QueryInterface<T> : IQueryInterface<T> where T : class
    {
        private readonly DatabaseContext _databaseContext;

        public QueryInterface(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _databaseContext.Set<T>().AsNoTracking().Where(predicate);
        }
    }

    public interface IQueryInterface<T>
    {
        IQueryable<T> Query(Expression<Func<T, bool>> predicate);
    }
}