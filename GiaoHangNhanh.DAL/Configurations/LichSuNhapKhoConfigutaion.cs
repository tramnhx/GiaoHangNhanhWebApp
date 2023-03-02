using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class LichSuNhapKhoConfigutaion : IEntityTypeConfiguration<LichSuNhapKho>
    {
        public void Configure(EntityTypeBuilder<LichSuNhapKho> builder)
        {
            builder.ToTable("LichSuNhapKhos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
