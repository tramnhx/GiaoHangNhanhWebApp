using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class DuyetChuyenHoan : BaseEntity
    {
        public string NguoiKyNhanChuyenHoan { get; set; }
        public bool IsDaDuyet { get; set; }
        public DateTime NgayDuyetChuyenHoan { get; set; }
        public int? VanDonId { get; set; }
        public VanDon VanDon { get; set; }
        public int? BuuCucDuyetHangId { get; set; }
        public BuuCuc BuuCucDuyetHang { get; set; }
        public int? DangKyChuyenHoanId { get; set; }
        public DangKyChuyenHoan DangKyChuyenHoan { get; set; }
    }
}
