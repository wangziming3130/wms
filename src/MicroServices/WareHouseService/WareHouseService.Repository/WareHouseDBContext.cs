using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService.Repository
{
    public class WareHouseDBContext : BaseDBContext
    {
        public WareHouseDBContext(DbContextOptions<WareHouseDBContext> options)
           : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WareHouseDBContext).Assembly);
            return;
        }
    }
}
