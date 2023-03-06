using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GiaoHangNhanh.DAL.Entities.Entity;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class LichSuHangDenConfiguration : IEntityTypeConfiguration<LichSuHangDen>
    {
        public void Configure(EntityTypeBuilder<LichSuHangDen> builder)
        {
            builder.ToTable("LichSuHangDens");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.BuuCuc).WithMany(x => x.LichSuHangDens).HasForeignKey(x => x.BuuCucId);
        }
    }
}
