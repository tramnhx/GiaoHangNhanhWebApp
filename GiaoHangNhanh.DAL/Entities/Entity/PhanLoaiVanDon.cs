namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class PhanLoaiVanDon : BaseEntity
    {
        public string MaVanDon { get; set; }
        public int IdDMPhanLoaiHangBT { get; set; }
        public VanDon VanDon { get; set; }
        public PhanLoaiHangBatThuong DanhMucPhanLoaiHangBT { get; set; }
    }
}
