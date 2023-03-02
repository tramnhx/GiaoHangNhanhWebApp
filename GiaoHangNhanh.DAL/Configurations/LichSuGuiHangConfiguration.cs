using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class LichSuGuiHangConfiguration : IEntityTypeConfiguration<LichSuGuiHang>
    {
        public void Configure(EntityTypeBuilder<LichSuGuiHang> builder)
        {
            builder.ToTable("LichSuGuiHangs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
