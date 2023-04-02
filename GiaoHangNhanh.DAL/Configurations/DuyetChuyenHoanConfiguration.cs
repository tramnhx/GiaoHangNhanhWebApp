using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiaoHangNhanh.DAL.Configurations
{
    public class DuyetChuyenHoanConfiguration : IEntityTypeConfiguration<DuyetChuyenHoan>
    {
        public void Configure(EntityTypeBuilder<DuyetChuyenHoan> builder)
        {
            builder.ToTable("DuyetChuyenHoans");
            builder.HasKey(x => x.Id);
        }
    }
}
