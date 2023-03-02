using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class BuuCuc : BaseEntity
    {
        public int? TinhId { get; set; }
        public int? HuyenId { get; set; }
        public int? KhuVucId { get; set; }
        public Tinh Tinh { get; set; }
        public Huyen Huyen { get; set; }
        public KhuVuc KhuVuc { get; set; }
        public ICollection<NhanVienChuyenPhat> NhanVienChuyenPhats { get; set; }
        public ICollection<KyNhan> KyNhans { get; set; }
        public ICollection<LichSuChuyenHang> LichSuChuyenHangs { get; set; }
        public ICollection<LichSuGuiHang> LichSuGuiHangs { get; set; }
        public ICollection<LichSuNhapKho> LichSuNhapKhos { get; set; }
        public ICollection<VanDon> VanDons { get; set; }

    }
}
