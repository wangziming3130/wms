
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.SqlServer.Migrations.Internal;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Core.Domain;

namespace Core.Service
{
    public class SqlServerMigrationDBRepository : SqlServerHistoryRepository, IMigrationHistoryRepository
    {
        private readonly ISqlProvider _sqlProvider;
        public SqlServerMigrationDBRepository(HistoryRepositoryDependencies dependencies) : base(dependencies)
        {
            _sqlProvider = SqlProviderFactory.Create(SqlType.SqlServer);
        }
        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);
            AutoMigrationHelper.ConfigureTable(history);
        }

        public int AddHistory(DbContext context, CDBMigrationInfo info)
        {
            return AutoMigrationHelper.AddHistory(context, info, _sqlProvider);
        }

        public string GetLastMigrationCodes(DbContext context)
        {
            return AutoMigrationHelper.GetLastMigrationCodes(context, _sqlProvider);
        }

        public Assembly ValueGenerationStrategyAssembly()
        {
            return typeof(SqlServerValueGenerationStrategy).Assembly;
        }
    }
}
