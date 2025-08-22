
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging.Debug;
using Core.Utility;

namespace Core.Service
{
    public abstract class BaseDBContext : DbContext
    {
        public static readonly LoggerFactory LoggerFactory = new LoggerFactory(new[] { new DebugLoggerProvider() });
        protected BaseDBContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(SiasunLogger.SiasunLoggerFactory);
            optionsBuilder.UseLoggerFactory(LoggerFactory);
            optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.ConfigureWarnings(
                w => w.Ignore(CoreEventId.DetachedLazyLoadingWarning,
                              CoreEventId.DuplicateDependentEntityTypeInstanceWarning,
                              CoreEventId.LazyLoadOnDisposedContextWarning));

            base.OnConfiguring(optionsBuilder);
        }
    }
}
