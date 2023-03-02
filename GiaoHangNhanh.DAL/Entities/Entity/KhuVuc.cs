using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class KhuVuc : BaseEntity
    {
        public int? HuyenId { get; set; }
        public Huyen Huyen { get; set; }
        public ICollection<BuuCuc> BuuCucs { get; set; }
        public ICollection<VanDon> VanDons { get; set; }
        public ICollection<CongTyGuiHang> CongTyGuiHangs { get; set; }
    }
}
