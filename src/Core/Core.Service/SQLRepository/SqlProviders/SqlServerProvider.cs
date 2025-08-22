using DotNetCore.CAP;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Core.Service
{
    public class SqlServerProvider : ISqlProvider
    {
        public IHealthChecksBuilder AddHealthChecker(IHealthChecksBuilder check, string connectionString, string name, IEnumerable<string> tags = null)
        {
            return check.AddSqlServer(connectionString, name: name, tags: tags);
        }

        public string GenerateInsertCmd(string table, Dictionary<string, object> item, out List<object> paramaters)
        {
            paramaters = null;
            if (item == null || item.Count == 0)
            {
                return "";
            }
            var format = "INSERT INTO {0}({1}) VALUES({2})";
            paramaters = item.Select(pair => GenerateParameter(pair.Key, pair.Value)).ToList();
            return string.Format(format,
                table,
                string.Join(",", item.Keys),
                string.Join(",", item.Keys.Select(k => $"@{k}"))
                );
        }

        public object GenerateParameter(string name, object value)
        {
            return new SqlParameter($"@{name}", value);
        }

        public string GenerateQueryLastItemCmd(string table, List<string> select, Dictionary<string, object> where, List<string> order, bool asc, out List<object> paramaters)
        {
            var format = "SELECT TOP 1 {0} FROM {1} {2} {3}";
            paramaters = where != null && where.Count > 0
                ? where.Select(pair => GenerateParameter(pair.Key, pair.Value)).ToList()
                : null;
            return string.Format(format,
                select != null && select.Count > 0 ? string.Join(",", select) : "*",
                table,
                where != null && where.Count > 0
                    ? "WHERE " + string.Join(" AND ", where.Select(pair => $"{pair.Key}=@{pair.Key}"))
                    : "",
                order != null && order.Count > 0
                    ? "ORDER BY " + string.Join(",", order) + (asc ? " ASC" : " DESC")
                    : ""
                );
        }

        public string GenerateCreateIndexCmd(string table, string indexName, List<string> indexColumns)
        {
            var format = "CREATE INDEX {0} ON {1} ({2});";
            return string.Format(format,
                indexName,
                table,
                string.Join(",", indexColumns)
                );
        }

        public DbContextOptionsBuilder Use(DbContextOptionsBuilder options, string connectionString)
        {
            return options.UseSqlServer(connectionString, x => { x.MigrationsHistoryTable(AutoMigrationHelper.Constants.TableMigrationHistories); })
                .ReplaceService<IHistoryRepository, SqlServerMigrationDBRepository>();
        }

        //public void ConfigCAPSqlOptions(CapOptions options, string connectionString, string capschema)
        //{
        //    //options.UseSqlServer(connectionString);
        //    options.UseSqlServer((config) => { config.ConnectionString = connectionString; config.Schema = capschema; });
        //}

        public string SqlTypeString()
        {
            return "SqlServer";
        }

        public string QuartzDriverDelegateType()
        {
            return "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz";
        }

        public IQueryable<T> Like<T>(IQueryable<T> query, string propertyName, string searchKeyword)
        {
            return Like(query, new List<string> { propertyName }, searchKeyword);
        }

        public IQueryable<T> Like<T>(IQueryable<T> query, List<string> propertyNames, string searchKeyword)
        {
            return query.Where(Like<T>(propertyNames, searchKeyword));
        }

        public Expression<Func<T, bool>> Like<T>(List<string> propertyNames, string searchKeyword)
        {
            var parameter = Expression.Parameter(typeof(T));
            var body = propertyNames.Select(propertyName => Expression.Call(typeof(DbFunctionsExtensions)
                .GetMethod(nameof(DbFunctionsExtensions.Like),
                    new[]
                    {
                        typeof(DbFunctions), typeof(string), typeof(string)
                    }),
                    Expression.Constant(EF.Functions),
                    SqlProviderHelper.GetMemberExpression(parameter, propertyName),
                    Expression.Constant(SqlProviderHelper.GetSearchKeyword(searchKeyword))))
                 .Aggregate<MethodCallExpression, Expression>(null, (current, call) =>
                    current != null ? Expression.OrElse(current, call) : (Expression)call);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
