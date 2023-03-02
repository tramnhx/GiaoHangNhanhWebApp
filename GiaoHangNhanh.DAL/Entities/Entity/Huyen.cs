using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class Huyen : BaseEntity
    {
        public int? TinhId { get; set; }
        public Tinh Tinh { get; set; }
        public ICollection<VanDon> VanDons { get; set; }
        public ICollection<KhuVuc> KhuVucs { get; set; }
        public ICollection<BuuCuc> BuuCucs { get; set; }
        public ICollection<CongTyGuiHang> CongTyGuiHangs { get; set; }
    }
}
