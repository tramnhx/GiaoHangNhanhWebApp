using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhuongThucThanhToans;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons
{
    public class VanDonDto : BaseDto
    {
        public string MoTaHangHoa { get; set; }
        public Decimal COD { get; set; }
        public DateTime NgayGuiHang { get; set; }
        public string StrNgayGuiHang { get; set; }
        public string NoiDungHangHoa { get; set; }

        // Thông tin người gửi
        public string HoTenNguoiGui { get; set; }
        public string DienThoaiNguoiGui { get; set; }
        public string DiaChiNguoiGui { get; set; }

        // Thông tin người nhận
        public string HoTenNguoiNhan { get; set; }
        public string DienThoaiNguoiNhan { get; set; }
        public string DiaChiNguoiNhan { get; set; }

        //Thông tin tính phí
        public double TrongLuong { get; set; }
        public decimal PhiBaoHiem { get; set; }
        public decimal GiaTriHangHoa { get; set; }

        public BuuCucDto BuuCuc { get; set; }
        public CongTyGuiHangDto CongTyGuiHang { get; set; }
        public DichVuDto DichVu { get; set; }
        public PhuongThucThanhToanDto PhuongThucThanhToan { get; set; }
        public UserDto User { get; set; }
    }
}
