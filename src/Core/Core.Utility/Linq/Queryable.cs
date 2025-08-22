using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class Queryable
    {
        public static List<T> NoLockRead<T>(this IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return query.ToList();
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, ReadUncommitted))
            //{
            //    var @return = query.ToList();
            //    scope.Complete();
            //    return @return;
            //}
        }

        public static T NoLockReadOne<T>(this IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return query.FirstOrDefault();

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, ReadUncommitted))
            //{
            //    var @return = query.FirstOrDefault();
            //    scope.Complete();
            //    return @return;
            //}
        }

        public static T NoLockReadOne<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return query.FirstOrDefault(predicate);

            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, ReadUncommitted))
            //{
            //    var @return = query.FirstOrDefault(predicate);
            //    scope.Complete();
            //    return @return;
            //}
        }

        public static Int32 NoLockCount<T>(this IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return query.Count();
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, ReadUncommitted))
            //{
            //    var @return = query.Count();
            //    scope.Complete();
            //    return @return;
            //}
        }
    }
}
