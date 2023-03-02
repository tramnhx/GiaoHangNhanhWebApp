using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class HuyenConfiguration : IEntityTypeConfiguration<Huyen>
    {
        public void Configure(EntityTypeBuilder<Huyen> builder)
        {
            builder.ToTable("DanhMucHuyens");
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Tinh).WithMany(x => x.Huyens).HasForeignKey(x => x.TinhId);
        }
    }
}