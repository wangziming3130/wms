
using Core.Domain;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface IAsyncDBRepository : IDisposable
    {
        public DbContext Context { get; set; }

        #region Create
        Task<T> AddAsync<T>(T entity, bool withSaveChange = true) where T : class;
        Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> entities, bool withSaveChange = true) where T : class;
        Task<int> AddRangeReturnCountAsync<T>(IEnumerable<T> entities, bool withSaveChange = true) where T : class;
        Task<T> AddHandleConcurrencyAsync<T>(T entity, bool withSaveChange = true) where T : class;
        #endregion

        #region Retrieve
        #region Single & FirstOrDefault
        Task<T> SingleAsync<T>(Expression<Func<T, bool>> expression, bool isTracking = false) where T : class;
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> expression, bool isTracking = false) where T : class;
        #endregion
        #region All
        DbSet<T> GetDbSet<T>() where T : class;
        IQueryable<T> AllWithTracking<T>(Expression<Func<T, bool>> expression = null) where T : class;
        IQueryable<T> AllWithNoTracking<T>(Expression<Func<T, bool>> expression = null) where T : class;
        IQueryable<T> All<T>(Expression<Func<T, bool>> expression = null, bool isTracking = false) where T : class;
        #endregion
        #region Condition
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> expression = null) where T : class;

        Task<bool> ContainsAsync<T>(Expression<Func<T, bool>> expression = null) where T : class;

        Task<int> Count<T>(Expression<Func<T, bool>> expression = null) where T : class;
        #endregion
        #region Find
        Task<T> FindAsync<T>(Guid id) where T : class;
        Task<T> FindAsync<T>(params object[] keys) where T : class;
        Task<T> FindAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        IQueryable<T> FindWithOrderBy<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order) where T : class;
        IQueryable<T> FindWithOrderByDescending<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order) where T : class;
        #endregion
        #region Get
        Task<TResult> GetOneWithSelectorAsync<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> selector) where T : class;
        IQueryable<TResult> GetAllWithSelector<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> selector) where T : class;
        Task<TableInfo<T>> GetAllWithPagingAsync<T, TResult>(TableQueryParameter<T, TResult> queryParameter) where T : class;
        #endregion
        #region Filter
        IQueryable<T> Filter<T>(Expression<Func<T, bool>> expression, bool isTracking = false) where T : class;
        IQueryable<T> FilterWithTracking<T>(Expression<Func<T, bool>> expression) where T : class;
        IQueryable<T> FilterWithNoTracking<T>(Expression<Func<T, bool>> expression) where T : class;
        IQueryable<TResult> Filter<T, TResult>(Expression<Func<T, TResult>> selector, bool isTracking = false) where T : class;
        IQueryable<TResult> Filter<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> selector, bool isTracking = false) where T : class;
        IQueryable<T> FilterPure<T>(Expression<Func<T, bool>> expression, List<string> navProps = null, bool isTracking = false) where T : class;
        IQueryable<T> FilterWithNavProps<T>(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> navProps = null, bool isTracking = false) where T : class;

        Task<T> FirstOrDefaultPureAsync<T>(Expression<Func<T, bool>> expression, List<string> navProps = null, bool isTracking = false) where T : class;
        Task<T> FirstOrDefaultPureAsync<T>(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> navProps = null, bool isTracking = false) where T : class;

        IQueryable<T> FilterWithOrderBy<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order, bool isTracking = false) where T : class;
        IQueryable<T> FilterWithOrderByDescending<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order, bool isTracking = false) where T : class;

        Task<FilterOutParamResult<T>> FilterAsync<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> orderBy, bool orderByASC, int pageIndex, int pageSize, bool isTracking = false) where T : class;
        Task<FilterOutParamResult<T>> FilterAsync<T>(Expression<Func<T, bool>> expression, int index = 0, int size = 50, bool isTracking = false) where T : class;
        #endregion

        Task LoadNavigationPropertyAsync<T>(T TObject, string navigationProperty) where T : class;
        #endregion

        #region Update
        Task<int> UpdateWithKeyAsync<T>(T TObject, string key = "Id", bool withSaveChange = true) where T : class;
        Task<int> UpdateRangeWithKeyAsync<T>(IEnumerable<T> TObjects, string key = "Id", bool withSaveChange = true) where T : class;
        Task<int> UpdateAsync<T>(T entity, bool withSaveChange = true) where T : class;
        Task<int> UpdateRangeAsync<T>(IEnumerable<T> entities, bool withSaveChange = true) where T : class;
        #endregion

        #region Delete
        Task<T> DeleteReturnEntityAsync<T>(T entity, bool withSaveChange = true) where T : class;
        Task<int> DeleteAsync<T>(T entity, bool withSaveChange = true) where T : class;
        Task<IEnumerable<T>> DeleteRangeReturnEntitiesAsync<T>(IEnumerable<T> entities, bool withSaveChange = true) where T : class;
        Task<int> DeleteRangeAsync<T>(IEnumerable<T> entities, bool withSaveChange = true) where T : class;
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> expression, bool withSaveChange = true) where T : class;
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        #endregion

        #region BySql
        IQueryable<T> FromSql<T>(string sql, params object[] param) where T : class;
        IQueryable<T> FromSql<T>(string formattedSql) where T : class;
        Task<List<T>> SqlQueryAsync<T>(string sql, params object[] param);
        Task<List<T>> SqlQueryWithDBOpenAsync<T>(string sql, params object[] param);

        Task<int> ExecuteSqlCommandAsync(string sql, params object[] param);
        Task<int> ExecuteSqlCommandAsync(string formattedSql);

        Task<int> BatchExecuteSqlCommandAsync(List<string> sqls);
        Task<int> BatchExecuteSqlCommandAsync(List<string> sqls, SqlParameter[] parameters);
        #endregion

        #region Other
        void EagerInitDatabase();
        void Execute(Action<DbContext> action);
        void Attach<T>(T obj) where T : class;

        void SetAutoDetectChangesEnabled(bool value);
        void DisableAutoDetectChanges();
        void EnableAutoDetectChanges();
        void DetectChanges();
        void ClearTracker();

        void SetCommandTimeout(int? second);
        void EnableLazyLoad();
        void DisableLazyLoad();
        #endregion

        #region Commit
        Task<int> CommitAsync();
        Task<int> CommitHandleConcurrencyAsync();
        Task<int> SaveChangesAsync(bool trackChange = true);
        Task<int> SaveChangesAsyncWithCancelToken(CancellationToken cancellationToken = default(CancellationToken));
        #endregion

        #region EFBatchOperation
        Task BatchInsertAsync<T>(IEnumerable<T> items, bool includeKey = true) where T : class;
        //Task BatchInsertForBaseEntityAsync<T>(IEnumerable<T> items, bool includeKey = true) where T : BaseEntity;
        Task BatchInsertAsync<T>(IEnumerable<T> items, BulkConfig bulkConfig) where T : class;

        Task BatchUpdateAsync<T>(IEnumerable<T> items, Action<UpdateSpecification<T>> updateSpecification) where T : class;
        Task BatchUpdateAsync<T>(IEnumerable<T> items, BulkConfig bulkConfig) where T : class;

        Task BatchDeleteAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task BatchDeleteAsync<T>(Expression<Func<T, bool>> expression, BulkConfig bulkConfig) where T : class;
        #endregion

        #region Bulk
        Task BulkAddAsync<T>(IEnumerable<T> source) where T : class;
        Task BulkUpdateAsync<S, T>(IEnumerable<S> source, Expression<Func<T, S, bool>> joinCondition, Expression<Func<Tuple<S, T>, T>> setter)
          where S : class
          where T : class;
        #endregion

        #region NoLock
        Task<T> SingleWithNoLockAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<T> FirstOrDefaultWithNoLockAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<List<T>> AllWithNoLockAsync<T>() where T : class;
        Task<bool> AnyWithNoLockAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<bool> ContainsWithNoLockAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<T> FindWithNoLockAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<T> FindWithNoLockPureAsync<T>(Expression<Func<T, bool>> expression, List<string> navProps = null) where T : class;
        Task<T> FindWithNoLockAsync<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order) where T : class;

        Task<List<T>> FilterWithNoLockAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<List<T>> FilterWithNoTrackingWithNoLockAsync<T>(Expression<Func<T, bool>> expression) where T : class;
        Task<List<TResult>> FilterWithNoLockAsync<T, TResult>(Expression<Func<T, TResult>> expression) where T : class;
        Task<List<TResult>> FilterWithNoLockAsync<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> selector) where T : class;
        Task<List<T>> FilterWithNoLockPureAsync<T>(Expression<Func<T, bool>> expression, List<string> navProps = null) where T : class;
        Task<List<T>> FilterWithNoLockPureAsync<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order) where T : class;
        Task<List<T>> FilterWithOrderByWithNoLockAsync<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order) where T : class;
        Task<List<T>> FilterWithOrderByDescendingWithNoLockAsync<T, TResult>(Expression<Func<T, bool>> expression, Expression<Func<T, TResult>> order, bool isTracting = false) where T : class;
        #endregion
    }
}
