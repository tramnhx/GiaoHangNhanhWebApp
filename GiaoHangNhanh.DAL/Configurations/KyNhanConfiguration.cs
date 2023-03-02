using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class KyNhanConfiguration : IEntityTypeConfiguration<KyNhan>
    {
        public void Configure(EntityTypeBuilder<KyNhan> builder)
        {
            builder.ToTable("KyNhans");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TenNguoiKy).HasMaxLength(100).IsRequired();

            builder.HasOne(x => x.BuuCuc).WithMany(x => x.KyNhans).HasForeignKey(x => x.BuuCucId);
        }
    }
}
