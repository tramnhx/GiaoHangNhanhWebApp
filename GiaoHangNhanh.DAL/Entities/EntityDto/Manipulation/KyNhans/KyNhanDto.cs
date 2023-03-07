using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans
{
    public class KyNhanDto : BaseDto
    {
        public int BuuCucId { get; set; }
        public int NhanVienCPId { get; set; }
        public string MaVanDon { get; set; }
        public string TenNguoiKy { get; set; }
        public bool DauKyThay { get; set; }
        public DateTime NgayKyNhan { get; set; }
        public BuuCuc DanhMucBuuCuc { get; set; }
        public NhanVien NhanVien { get; set; }
        public VanDon VanDon { get; set; }
    }
}
