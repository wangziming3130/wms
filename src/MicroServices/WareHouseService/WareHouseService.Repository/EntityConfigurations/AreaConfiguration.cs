using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WareHouseService.Repository;

namespace WareHouseService.Repository
{
    class AreaConfiguration : IEntityTypeConfiguration<AreaEntity>
    {
        public void Configure(EntityTypeBuilder<AreaEntity> builder)
        {
            builder.ToTable(AreaConstants.AreaTableName);
            builder.HasKey(a => a.AREA_ID);
            builder.Property(a => a.AREA_ID).HasColumnName(AreaConstants.AreaIdColumn).ValueGeneratedOnAdd();
            
            builder.Property(a => a.AREA_NAME).HasColumnName(AreaConstants.AreaNameColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.AREA_CODE).HasColumnName(AreaConstants.AreaCodeColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.AREA_REMARK).HasColumnName(AreaConstants.AreaRemarkColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.CREATE_TIME).HasColumnName(AreaConstants.AreaCreateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);
            builder.Property(a => a.UPDATE_TIME).HasColumnName(AreaConstants.AreaUpdateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);


            builder.Property(a => a.WAREHOUSE_ID).HasColumnName(AreaConstants.WareHouseIdColumn).IsRequired();
            builder.HasOne(l => l.WAREHOUSE).WithMany().HasForeignKey(l => l.WAREHOUSE_ID).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
