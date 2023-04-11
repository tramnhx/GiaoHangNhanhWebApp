using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class LichSuChuyenHangConfiguration : IEntityTypeConfiguration<LichSuChuyenHang>
    {
        public void Configure(EntityTypeBuilder<LichSuChuyenHang> builder)
        {
            builder.ToTable("LichSuChuyenHangs");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SealXe).HasMaxLength(50).IsRequired();

            builder.HasOne(x => x.BuuCuc).WithMany(x => x.LichSuChuyenHangs).HasForeignKey(x => x.BuuCucId);

        }
    }
}
