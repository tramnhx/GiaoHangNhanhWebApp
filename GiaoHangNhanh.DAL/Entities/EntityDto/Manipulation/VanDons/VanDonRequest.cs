using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons
{
    public class VanDonRequest
    {
        public int? Id { get; set; }
        public string Code { set; get; }
        public string NhanVienLayHangId { get; set; }
        public int DichVuId { get; set; }
        public int CongTyGuiHangId { get; set; }
        public int PhuongThucThanhToanId { get; set; }
        public string MoTaHangHoa { get; set; }
        public decimal GiaTriHangHoa { get; set; }
        public int BuuCucHangDenId { get; set; }
        public double TrongLuong { get; set; }
        public decimal COD { get; set; }
        public string NgayGuiHang { get; set; }
        public string NoiDungHangHoa { get; set; }

        // Thông tin người gửi
        public string HoTenNguoiGui { get; set; }
        public string DienThoaiNguoiGui { get; set; }
        public string DiaChiNguoiGui { get; set; }
        public bool IsDefault { set; get; }
        public int SortOrder { set; get; }

        // Thông tin người nhận
        public string HoTenNguoiNhan { get; set; }
        public string DienThoaiNguoiNhan { get; set; }
        public string DiaChiNguoiNhan { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public string CreatedUserId { set; get; }
        public string ModifiedUserId { set; get; }
    }
}
