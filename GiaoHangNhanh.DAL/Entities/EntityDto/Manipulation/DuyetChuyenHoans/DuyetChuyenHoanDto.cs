using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DuyetChuyenHoans
{
    public class DuyetChuyenHoanDto : BaseDto
    {
        public int? DangKyChuyenHoanId { get; set; }
        public int? VanDonId { get; set; }
        public string NguoiKyNhanChuyenHoan { get; set; }
        public string IsDaDuyet { get; set; }
        public string StrNgayDuyetChuyenHoan { get; set; }
        public VanDonDto VanDon { get; set; }
        public DangKyChuyenHoanDto DangKyChuyenHoan { get; set; }
        public BuuCucDto BuuCuc { get; set; }
        public string MaVanDon { get; set; }
        public string BuuCucGuiHang { get; set; }
        public string PhanLoaiChuyenPhat { get; set; }
        public string TenKhachHang { get; set; }
        public string RutVeDichDen { get; set; }
        public string DangKyBuuCuc { get; set; }
        public string NguyenNhanChuyenHoan { get; set; }
        public string BuuCucDuyetChuyenHoan { get; set; }
        public string XacNhanBuuCuc { get; set; }


    }
}
