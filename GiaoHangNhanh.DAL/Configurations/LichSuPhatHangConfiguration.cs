using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;

namespace GiaoHangNhanh.DAL.Configurations 
{
    public class LichSuPhatHangConfiguration : IEntityTypeConfiguration<LichSuPhatHang>
    {
        public void Configure(EntityTypeBuilder<LichSuPhatHang> builder)
        {
            builder.ToTable("LichSuPhatHangs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasOne(x => x.VanDon).WithMany(x => x.LichSuPhatHangs).HasForeignKey(x => x.VanDonId).HasPrincipalKey(x => x.Id);
            builder.HasOne(x => x.NhanVien).WithMany(x => x.LichSuPhatHangs).HasForeignKey(x => x.NhanVienId).HasPrincipalKey(x => x.Id);
        }
    }
}