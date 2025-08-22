
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Core.Service
{
    public interface IMigrationHistoryRepository
    {
        int AddHistory(DbContext context, CDBMigrationInfo info);

        string GetLastMigrationCodes(DbContext context);

        Assembly ValueGenerationStrategyAssembly();
    }
}
