using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class VanDonConfiguration : IEntityTypeConfiguration<VanDon>
    {
        public void Configure(EntityTypeBuilder<VanDon> builder)
        {
            builder.ToTable("VanDons");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.NoiDungHangHoa).IsRequired(false);
            builder.Property(x => x.HoTenNguoiGui).IsRequired(false);
            builder.Property(x => x.DienThoaiNguoiGui).IsRequired(false);
            builder.Property(x => x.DiaChiNguoiGui).IsRequired(false);

            builder.Property(x => x.HoTenNguoiNhan).HasMaxLength(250).IsRequired();
            builder.Property(x => x.DienThoaiNguoiNhan).HasMaxLength(20).IsRequired();
            builder.Property(x => x.DiaChiNguoiNhan).IsRequired();

            builder.HasOne(x => x.BuuCuc).WithMany(x => x.VanDons).HasForeignKey(x => x.BuuCucHangDenId).HasPrincipalKey(x => x.Id);
            builder.HasOne(x => x.CongTyGuiHang).WithMany(x => x.VanDons).HasForeignKey(x => x.CongTyGuiHangId);
            builder.HasOne(x => x.DichVu).WithMany(x => x.VanDons).HasForeignKey(x => x.DichVuId);
            builder.HasOne(x => x.PhuongThucThanhToan).WithMany(x => x.VanDons).HasForeignKey(x => x.PhuongThucThanhToanId);
            builder.HasOne(x => x.NhanVien).WithMany(x => x.VanDons).HasForeignKey(x => x.NhanVienId);
        }
    }
}
