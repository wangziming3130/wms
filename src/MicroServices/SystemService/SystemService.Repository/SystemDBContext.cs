using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemService.Repository
{
    public class SystemDBContext : BaseDBContext
    {
        public SystemDBContext(DbContextOptions<SystemDBContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SystemDBContext).Assembly);
            return;
        }
    }
}
