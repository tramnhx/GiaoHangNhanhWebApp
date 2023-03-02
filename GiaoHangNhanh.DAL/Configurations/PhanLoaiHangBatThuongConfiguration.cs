using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class PhanLoaiHangBatThuongConfiguration : IEntityTypeConfiguration<PhanLoaiHangBatThuong>
    {
        public void Configure(EntityTypeBuilder<PhanLoaiHangBatThuong> builder)
        {
            builder.ToTable("DanhMucPhanLoaiHangBatThuongs");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        }
    }
}
