using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    class TinhConfiguration : IEntityTypeConfiguration<Tinh>
    {
        public void Configure(EntityTypeBuilder<Tinh> builder)
        {
            builder.ToTable("DanhMucTinhs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200);
        }
    }
}
