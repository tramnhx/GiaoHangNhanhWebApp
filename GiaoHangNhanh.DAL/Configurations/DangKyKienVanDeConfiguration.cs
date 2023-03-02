using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class DangKyKienVanDeConfiguration : IEntityTypeConfiguration<DangKyKienVanDe>
    {
        public void Configure(EntityTypeBuilder<DangKyKienVanDe> builder)
        {
            builder.ToTable("DangKyKienVanDes");
            builder.HasKey(x => x.Id);

            builder.HasOne(v => v.VanDon).WithMany(p => p.DangKyKienVanDes).HasForeignKey(p => p.Id);

            builder.HasOne(p => p.PhanLoaiHangBatThuong).WithMany(p => p.DangKyKienVanDes).HasForeignKey(p => p.Id);

        }
    }
}
