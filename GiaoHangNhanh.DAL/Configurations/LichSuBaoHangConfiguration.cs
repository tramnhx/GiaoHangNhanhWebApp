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
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.SealBao).HasMaxLength(50).IsRequired(false);
        }
    }
}
