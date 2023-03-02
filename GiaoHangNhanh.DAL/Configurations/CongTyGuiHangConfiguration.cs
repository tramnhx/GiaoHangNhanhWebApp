using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class CongTyGuiHangConfiguration : IEntityTypeConfiguration<CongTyGuiHang>
    {
        public void Configure(EntityTypeBuilder<CongTyGuiHang> builder)
        {
            builder.ToTable("DanhMucCongTyGuiHangs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.DiaChi).HasMaxLength(500).IsRequired(false);

            builder.HasOne(x => x.Tinh).WithMany(x => x.CongTyGuiHangs).HasForeignKey(x => x.TinhId);
            builder.HasOne(x => x.Huyen).WithMany(x => x.CongTyGuiHangs).HasForeignKey(x => x.HuyenId);
            builder.HasOne(x => x.KhuVuc).WithMany(x => x.CongTyGuiHangs).HasForeignKey(x => x.KhuVucId);
        }
    }
}
