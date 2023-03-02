using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class DichVuConfiguration : IEntityTypeConfiguration<DichVu>
    {
        public void Configure(EntityTypeBuilder<DichVu> builder)
        {
            builder.ToTable("DanhMucDichVus");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Code).HasMaxLength(200).IsRequired();
        }
    }
}
