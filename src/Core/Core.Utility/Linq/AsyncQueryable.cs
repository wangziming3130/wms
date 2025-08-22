using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class AsyncQueryable
    {
        public static async Task<List<T>> NoLockReadAsync<T>(this IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return await query.ToListAsync();
        }

        public static async Task<T> NoLockReadOneAsync<T>(this IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return await query.FirstOrDefaultAsync();
        }

        public static async Task<T> NoLockReadOneAsync<T>(this IQueryable<T> query, Expression<Func<T, bool>> predicate)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return await query.FirstOrDefaultAsync(predicate);
        }

        public static async Task<Int32> NoLockCountAsync<T>(this IQueryable<T> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            return await query.CountAsync();
        }
    }
}
