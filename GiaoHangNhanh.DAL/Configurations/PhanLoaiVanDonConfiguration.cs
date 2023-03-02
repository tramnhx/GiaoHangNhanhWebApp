using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class PhanLoaiVanDonConfiguration : IEntityTypeConfiguration<PhanLoaiVanDon>
    {
        public void Configure(EntityTypeBuilder<PhanLoaiVanDon> builder)
        {
            builder.ToTable("PhanLoaiVanDons");
            //builder.HasKey(x => new { x.Id, x.MaVanDon, x.IdDMPhanLoaiHangBT });

            //builder.HasOne(v => v.VanDon).WithMany(p => p.PhanLoaiHangBatThuongs).HasForeignKey(p => p.MaVanDon).HasPrincipalKey(x => x.Code);

            //builder.HasOne(pdm => pdm.DanhMucPhanLoaiHangBT).WithMany(p => p.PhanLoaiHangBatThuongs).HasForeignKey(p => p.IdDMPhanLoaiHangBT);
        }
    }
}
