using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class KyNhan : BaseEntity
    {
        public int BuuCucId { get; set; }
        public int VanDonId { get; set; }
        public Guid NhanVietPhat { get; set; }
        public string TenNguoiKy { get; set; }
        public bool DauKyThay { get; set; }
        public DateTime NgayKyNhan { get; set; }
        public BuuCuc BuuCuc { get; set; }      
        public VanDon VanDon { get; set; }
        public AppUser AppUser { get; set; }
    }
}
