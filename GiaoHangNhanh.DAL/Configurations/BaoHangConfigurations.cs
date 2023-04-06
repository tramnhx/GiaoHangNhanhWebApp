using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GiaoHangNhanh.DAL.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class BaoHangConfigurations : IEntityTypeConfiguration<BaoHang>
    {
        public void Configure(EntityTypeBuilder<BaoHang> builder)
        {
            builder.ToTable("BaoHangs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        }
    }
}
