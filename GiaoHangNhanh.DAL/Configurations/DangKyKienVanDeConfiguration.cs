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

            builder.HasOne(x => x.VanDon).WithMany(x => x.DangKyKienVanDes).HasForeignKey(x => x.VanDonId);

            builder.HasOne(x => x.PhanLoaiHangBatThuong).WithMany(x => x.DangKyKienVanDes).HasForeignKey(x => x.PhanLoaiHangBatThuongId);

        }
    }
}
