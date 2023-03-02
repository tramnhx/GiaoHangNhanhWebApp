using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class BuuCucConfiguration : IEntityTypeConfiguration<BuuCuc>
    {
        public void Configure(EntityTypeBuilder<BuuCuc> builder)
        {
            builder.ToTable("DanhMucBuuCucs");
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Code).HasMaxLength(15);
            builder.HasKey(x => x.Id );
            builder.HasOne(x => x.Tinh).WithMany(x => x.BuuCucs).HasForeignKey(x => x.TinhId);
            builder.HasOne(x => x.Huyen).WithMany(x => x.BuuCucs).HasForeignKey(x => x.HuyenId);
            builder.HasOne(x => x.KhuVuc).WithMany(x => x.BuuCucs).HasForeignKey(x => x.KhuVucId);
        }
    }
}
