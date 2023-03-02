using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    class NhanVienChuyenPhatConfiguration : IEntityTypeConfiguration<NhanVienChuyenPhat>
    {
        public void Configure(EntityTypeBuilder<NhanVienChuyenPhat> builder)
        {
            builder.ToTable("NhanVienChuyenPhats");
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.HasKey(x => new { x.Id, x.Code });
            builder.HasOne(x => x.DanhMucBuuCuc).WithMany(x => x.NhanVienChuyenPhats).HasForeignKey(x => x.BuuCucId).HasPrincipalKey(x => x.Id);
        }
    }
}
