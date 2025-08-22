using DotNetCore.CAP;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public interface ISqlProvider
    {
        DbContextOptionsBuilder Use(DbContextOptionsBuilder options, string connectionString);

        IHealthChecksBuilder AddHealthChecker(IHealthChecksBuilder check, string connectionString, string name, IEnumerable<string> tags = null);

        string GenerateInsertCmd(string table, Dictionary<string, object> item, out List<object> paramaters);

        string GenerateQueryLastItemCmd(string table, List<string> select, Dictionary<string, object> where, List<string> order, bool asc, out List<object> paramaters);
        string GenerateCreateIndexCmd(string table, string indexName, List<string> indexColumns);
        object GenerateParameter(string name, object value);
        //void ConfigCAPSqlOptions(CapOptions options, string connectionString, string capschema);
        string SqlTypeString();
        string QuartzDriverDelegateType();
        IQueryable<T> Like<T>(IQueryable<T> query, string propertyName, string searchKeyword);
        IQueryable<T> Like<T>(IQueryable<T> query, List<string> propertyNames, string searchKeyword);
        Expression<Func<T, bool>> Like<T>(List<string> propertyNames, string searchKeyword);
    }
}
