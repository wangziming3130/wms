using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using SystemService.Repository;

namespace SystemService.Repository
{
    class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable(UserConstants.UserTableName);
            builder.HasKey(a => a.USER_ID);
            builder.Property(a => a.USER_ID).HasColumnName(UserConstants.UserIdColumn).ValueGeneratedOnAdd();
            builder.Property(a => a.USER_NAME).HasColumnName(UserConstants.UserNameColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.USER_ACCOUNT).HasColumnName(UserConstants.UserAccountColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.USER_PASSWORD).HasColumnName(UserConstants.UserPasswordColumn).HasMaxLength(255).IsRequired();
            builder.Property(a => a.USER_AVATAR).HasColumnName(UserConstants.UserAvatarColumn).IsRequired();
            //builder.Property(a => a.TEST_INFO).HasColumnName("123").IsRequired();
            builder.Property(a => a.USER_PASSWORD_TIME).HasColumnName(UserConstants.UserPasswordTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);
            builder.Property(a => a.CREATE_TIME).HasColumnName(UserConstants.UserCreateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);
            builder.Property(a => a.UPDATE_TIME).HasColumnName(UserConstants.UserUpdateTimeColumn).HasColumnType(nameof(SqlDbType.DateTime2)).IsRequired(false);

        }
    }
}
