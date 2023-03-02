using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class DangKyChuyenHoanConfiguration : IEntityTypeConfiguration<DangKyChuyenHoan>
    {
        public void Configure(EntityTypeBuilder<DangKyChuyenHoan> builder)
        {
            builder.ToTable("DangKyChuyenHoans");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).UseIdentityColumn();

            builder.Property(x => x.NguyenNhan).HasMaxLength(200).IsRequired();
        }
    }
}
