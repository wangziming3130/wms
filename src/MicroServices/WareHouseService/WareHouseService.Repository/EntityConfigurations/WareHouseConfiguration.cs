using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using WareHouseService.Repository;

namespace WareHouseService.Repository
{
    class WareHouseConfiguration : IEntityTypeConfiguration<WareHouseEntity>
    {
        public void Configure(EntityTypeBuilder<WareHouseEntity> builder)
        {
            builder.ToTable(WareHouseConstants.WareHouseTableName);
            builder.HasKey(a => a.WAREHOUSE_ID);
            builder.Property(a => a.WAREHOUSE_ID).HasColumnName(WareHouseConstants.WareHouseIdColumn).ValueGeneratedOnAdd();
            builder.Property(a => a.WAREHOUSE_CODE).HasColumnName(WareHouseConstants.WareHouseCodeColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.WAREHOUSE_NAME).HasColumnName(WareHouseConstants.WareHouseTableName).HasMaxLength(255).IsRequired();
            builder.Property(a => a.WAREHOUSE_REMARK).HasColumnName(WareHouseConstants.WareHouseRemarkColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.CREATE_TIME).HasColumnName(WareHouseConstants.WareHouseCreateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);
            builder.Property(a => a.UPDATE_TIME).HasColumnName(WareHouseConstants.WareHouseUpdateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);

        }
    }
}
