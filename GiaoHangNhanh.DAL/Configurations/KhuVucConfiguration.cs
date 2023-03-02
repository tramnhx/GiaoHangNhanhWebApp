using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class KhuVucConfiguration : IEntityTypeConfiguration<KhuVuc>
    {
        public void Configure(EntityTypeBuilder<KhuVuc> builder)
        {
            builder.ToTable("DanhMucKhuVucs");
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Huyen).WithMany(x => x.KhuVucs).HasForeignKey(x => x.HuyenId);
        }
    }
}
