using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class Tinh : BaseEntity
    {
        public ICollection<VanDon> VanDons { get; set; }
        public ICollection<Huyen> Huyens { get; set; }
        public ICollection<BuuCuc> BuuCucs { get; set; }
        public ICollection<CongTyGuiHang> CongTyGuiHangs { get; set; }
    }
}
