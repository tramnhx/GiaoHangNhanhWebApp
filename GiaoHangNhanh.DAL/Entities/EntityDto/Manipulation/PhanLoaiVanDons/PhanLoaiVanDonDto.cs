using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.PhanLoaiVanDons
{
    public class PhanLoaiVanDonDto : BaseDto
    {
        public string MaVanDon { get; set; }
        public int IdDMPhanLoaiHangBT { get; set; }
        public PhanLoaiVanDon PhanLoaiHangBatThuong { get; set; }
        public VanDon VanDon { get; set; }
    }
}
