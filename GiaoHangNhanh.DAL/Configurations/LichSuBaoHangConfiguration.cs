using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class LichSuBaoHangConfiguration : IEntityTypeConfiguration<LichSuBaoHang>
    {
        public void Configure(EntityTypeBuilder<LichSuBaoHang> builder)
        {
            builder.ToTable("LichSuBaoHangs");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.VanDon).WithMany(x => x.LichSuBaoHangs).HasForeignKey(x => x.VanDonId);
            builder.HasOne(x => x.BaoHang).WithMany(x => x.LichSuBaoHangs).HasForeignKey(x => x.BaoHangId);
        }
    }
}
