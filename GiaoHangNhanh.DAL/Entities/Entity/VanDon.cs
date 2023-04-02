using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class VanDon : BaseEntity
    {
        public DateTime NgayGuiHang { get; set; }

        // Thông tin mặt hàng
        public string NoiDungHangHoa { get; set; }
        public decimal GiaTriHangHoa { get; set; }

        // Thông tin tính phí
        public double TrongLuong { get; set; }
        //COD
        public decimal COD { get; set; }
        // Thông tin người gửi
        public string HoTenNguoiGui { get; set; }
        public string DienThoaiNguoiGui { get; set; }
        public string DiaChiNguoiGui { get; set; }
        // Thông tin người nhận
        public string HoTenNguoiNhan { get; set; }
        public string DienThoaiNguoiNhan { get; set; }
        public string DiaChiNguoiNhan { get; set; }

        public Guid NhanVienLayHangId { get; set; }
        public int? BuuCucHangDenId { get; set; }
        public BuuCuc BuuCuc { get; set; }
        public int? CongTyGuiHangId { get; set; }
        public CongTyGuiHang CongTyGuiHang { get; set; }
        public int? DichVuId { get; set; }
        public DichVu DichVu { get; set; }
        public int? PhuongThucThanhToanId { get; set; }
        public PhuongThucThanhToan PhuongThucThanhToan { get; set; }

        public ICollection<DangKyKienVanDe> DangKyKienVanDes { get; set; }
        public ICollection<LichSuBaoHang> LichSuBaoHangs { get; set; }
        public ICollection<LichSuChuyenHang> LichSuChuyenHangs { get; set; }
        public ICollection<LichSuPhatHang> LichSuPhatHangs { get; set; }
        public ICollection<DuyetChuyenHoan> DuyetChuyenHoans { get; set; }


    }
}
