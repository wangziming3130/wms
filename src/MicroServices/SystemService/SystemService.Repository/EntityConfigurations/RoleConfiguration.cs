using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemService.Repository;

namespace SystemService.Repository
{
    class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable(RoleConstants.RoleTableName);
            builder.HasKey(a => a.ROLE_ID);
            builder.Property(a => a.ROLE_ID).HasColumnName(RoleConstants.RoleIdColumn).ValueGeneratedOnAdd();
            builder.Property(a => a.ROLE_NAME).HasColumnName(RoleConstants.RoleTableName).HasMaxLength(255).IsRequired();
            builder.Property(a => a.ROLE_REMARK).HasColumnName(RoleConstants.RoleRemarkColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.CREATE_TIME).HasColumnName(RoleConstants.RoleCreateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);
            builder.Property(a => a.UPDATE_TIME).HasColumnName(RoleConstants.RoleUpdateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);

        }
    }
}
