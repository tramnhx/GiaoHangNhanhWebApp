using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class KyNhan : BaseEntity
    {
        public KyNhan()
        {
            DauKyThay = false;
        }
        public int? VanDonId { get; set; }
        public Guid NhanVienPhat { get; set; }
        public string TenNguoiKy { get; set; }
        public bool DauKyThay { get; set; }
        public DateTime NgayKyNhan { get; set; }
        public int? BuuCucId { get; set; }
        public BuuCuc BuuCuc { get; set; }
        public AppUser AppUser { get; set; }
    }
}
