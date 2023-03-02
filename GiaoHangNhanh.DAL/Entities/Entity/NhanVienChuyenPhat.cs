using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class NhanVienChuyenPhat : BaseEntity
    {
        public int BuuCucId { get; set; }
        public BuuCuc DanhMucBuuCuc { get; set; }

        public ICollection<KyNhan> KyNhans { get; set; }
        public ICollection<LichSuNhapKho> LichSuNhapKhos { get; set; }
        public ICollection<VanDon> VanDons { get; set; }
    }
}
