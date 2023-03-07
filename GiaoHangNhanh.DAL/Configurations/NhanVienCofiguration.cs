using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class NhanVienConfiguration : IEntityTypeConfiguration<NhanVien>
    {
        public void Configure(EntityTypeBuilder<NhanVien> builder)
        {
            builder.ToTable("NhanViens");
            builder.HasOne(x => x.Gender).WithMany(x => x.NhanViens).HasForeignKey(x => x.GenderId);
            builder.HasOne(x => x.BuuCuc).WithMany(x => x.NhanViens).HasForeignKey(x => x.BuuCucLamViecId);
        }
    }
}
