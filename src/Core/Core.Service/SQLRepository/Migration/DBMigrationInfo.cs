
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Service
{
    public class CDBMigrationInfo : HistoryRow
    {
        public string ContextFullname { get; set; }
        public DateTime MigrationTime { get; set; }
        public string MigrationCodes { get; set; }
        public CDBMigrationInfo(string migrationId, string productVersion, string contextFullname, string mirationCode) : base(migrationId, productVersion)
        {
            ContextFullname = contextFullname;
            MigrationCodes = mirationCode;
            MigrationTime = DateTime.UtcNow;
        }
    }

    public class AutoMigrationHelper
    {
        public class Constants
        {
            public const string TableMigrationHistories = "_MigrationHistories";
            public const string ColumnMigrationId = "MigrationId";
            public const string ColumnProductVersion = "ProductVersion";
            public const string ColumnContextFullname = "ContextFullname";
            public const string ColumnMigrationTime = "MigrationTime";
            public const string ColumnMigrationCodes = "MigrationCodes";

            public static readonly string InsertCommandSQL = $"INSERT INTO {TableMigrationHistories}({ColumnMigrationId},{ColumnProductVersion},{ColumnContextFullname},{ColumnMigrationTime},{ColumnMigrationCodes})" +
                $" VALUES (@{ColumnMigrationId}, @{ColumnProductVersion}, @{ColumnContextFullname}, @{ColumnMigrationTime},@{ColumnMigrationCodes})";

            public static readonly string InsertCommandPgSQL = $"INSERT INTO \"{TableMigrationHistories}\"(\"{ColumnMigrationId}\",\"{ColumnProductVersion}\",\"{ColumnContextFullname}\",\"{ColumnMigrationTime}\",\"{ColumnMigrationCodes}\")" +
                $" VALUES (@{ColumnMigrationId}, @{ColumnProductVersion}, @{ColumnContextFullname}, @{ColumnMigrationTime},@{ColumnMigrationCodes})";

            public static readonly string GetLastItemSQL = $"select top 1 {ColumnMigrationCodes} from {TableMigrationHistories} where {ColumnContextFullname} = @{ColumnContextFullname} order by {ColumnMigrationTime} desc";

            public static readonly string GetLastItemPgSQL = $"select \"{ColumnMigrationCodes}\" from \"{TableMigrationHistories}\" where \"{ColumnContextFullname}\" = @{ColumnContextFullname} order by \"{ColumnMigrationTime}\" desc LIMIT 1";
        }

        public static void ConfigureTable(EntityTypeBuilder<HistoryRow> historyBuilder)
        {
            historyBuilder.ToTable(Constants.TableMigrationHistories);
            historyBuilder.Property<string>(Constants.ColumnContextFullname).HasMaxLength(128).IsRequired();
            historyBuilder.Property<DateTime>(Constants.ColumnMigrationTime).IsRequired();
            historyBuilder.Property<string>(Constants.ColumnMigrationCodes).IsRequired();
        }

        public static string ExecuteRead(DbContext db, string commandLine, params object[] commandParams)
        {
            var result = "";
            using (var command = db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = commandLine;
                command.CommandType = CommandType.Text;
                if (commandParams != null && commandParams.Any())
                {
                    command.Parameters.AddRange(commandParams);
                }
                db.Database.OpenConnection();
                using (var readerResult = command.ExecuteReader())
                {
                    if (readerResult.Read())
                    {
                        result = readerResult.GetString(0);
                    }
                }
            }
            return result;
        }

        public static int AddHistory(DbContext context, CDBMigrationInfo info, ISqlProvider sqlProvider)
        {
            string cmd = sqlProvider.GenerateInsertCmd(
                Constants.TableMigrationHistories,
                new Dictionary<string, object>()
                {
                    { Constants.ColumnMigrationId,  info.MigrationId },
                    { Constants.ColumnProductVersion,  info.ProductVersion },
                    { Constants.ColumnContextFullname,  info.ContextFullname },
                    { Constants.ColumnMigrationTime,  info.MigrationTime },
                    { Constants.ColumnMigrationCodes,  info.MigrationCodes }
                },
                out List<object> paramaters);
            return context.Database.ExecuteSqlRaw(cmd, paramaters.ToArray());
        }

        public static string GetLastMigrationCodes(DbContext context, ISqlProvider sqlProvider)
        {
            string cmd = sqlProvider.GenerateQueryLastItemCmd(
                Constants.TableMigrationHistories,
                new List<string> { Constants.ColumnMigrationCodes },
                new Dictionary<string, object> { { Constants.ColumnContextFullname, context.GetType().FullName } },
                new List<string> { Constants.ColumnMigrationTime },
                false,
                out List<object> paramaters
                );
            return ExecuteRead(context, cmd, paramaters.ToArray());
        }

        public static bool IsDBReady(DbContext context)
        {
            var sqlProvider = SqlProviderFactory.Create();
            var lastModuleCodes = AutoMigrationHelper.GetLastMigrationCodes(context, sqlProvider);
            var isReady = !string.IsNullOrWhiteSpace(lastModuleCodes);
            return isReady;

        }

        //public static List<object> GetParameterList(CDBMigrationInfo info, string sqlType)
        //{
        //    var parameters = new List<object>();
        //    switch (sqlType)
        //    {
        //        case Domain.Constants.SqlType.PostgreSql:
        //            {
        //                parameters.Add(new NpgsqlParameter($"@{Constants.ColumnMigrationId}", info.MigrationId));
        //                parameters.Add(new NpgsqlParameter($"@{Constants.ColumnProductVersion}", info.ProductVersion));
        //                parameters.Add(new NpgsqlParameter($"@{Constants.ColumnContextFullname}", info.ContextFullname));
        //                parameters.Add(new NpgsqlParameter($"@{Constants.ColumnMigrationTime}", info.MigrationTime));
        //                parameters.Add(new NpgsqlParameter($"@{Constants.ColumnMigrationCodes}", info.MigrationCodes));
        //            }; break;
        //        case Domain.Constants.SqlType.SqlServer:
        //        default:
        //            {
        //                parameters.Add(new SqlParameter($"@{Constants.ColumnMigrationId}", info.MigrationId));
        //                parameters.Add(new SqlParameter($"@{Constants.ColumnProductVersion}", info.ProductVersion));
        //                parameters.Add(new SqlParameter($"@{Constants.ColumnContextFullname}", info.ContextFullname));
        //                parameters.Add(new SqlParameter($"@{Constants.ColumnMigrationTime}", info.MigrationTime));
        //                parameters.Add(new SqlParameter($"@{Constants.ColumnMigrationCodes}", info.MigrationCodes));
        //            }; break;

        //    }
        //    return parameters;
        //}
    }
}
