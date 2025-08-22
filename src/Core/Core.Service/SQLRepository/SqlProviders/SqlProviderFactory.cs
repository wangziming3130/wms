using Core.Domain;
using Core.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class SqlProviderFactory
    {
        public static ISqlProvider Create()
        {
            var type = RuntimeContext.Config.DBConnections.DBType;
            return Create(type);
        }

        public static ISqlProvider Create(DbContext context)
        {
            //if (context.Database.IsNpgsql())
            //{
            //    return new PostgreSqlProvider();
            //}
            //else if (context.Database.IsSqlServer())
            //{
            //    return new SqlServerProvider();
            //}
            return new SqlServerProvider();
        }

        public static ISqlProvider Create(string type)
        {
            ISqlProvider result = (type) switch
            {
                SqlType.SqlServer => new SqlServerProvider(),
                //SqlType.PostgreSql => new PostgreSqlProvider(),
                _ => new SqlServerProvider(),
            };
            return result;
        }
    }
}
