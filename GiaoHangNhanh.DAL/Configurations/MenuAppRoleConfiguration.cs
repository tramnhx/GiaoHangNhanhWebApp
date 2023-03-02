using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class MenuAppRoleConfiguration : IEntityTypeConfiguration<MenuAppRole>
    {
        public void Configure(EntityTypeBuilder<MenuAppRole> builder)
        {
            builder.ToTable("MenuAppRoles");

            builder.HasKey(x => new { x.AppRoleId, x.MenuId, x.Id });
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.Menu).WithMany(x => x.MenuAppRoles).HasForeignKey(x => x.MenuId);
            builder.HasOne(x => x.AppRole).WithMany(x => x.MenuAppRoles).HasForeignKey(x => x.AppRoleId);
        }
    }
}