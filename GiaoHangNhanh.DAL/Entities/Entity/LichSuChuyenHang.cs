using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class LichSuChuyenHang : BaseEntity
    {
        public int? BuuCucId { get; set; }
        public BuuCuc BuuCuc { get; set; }
        public string SealXe { get; set; }
        public bool IsXeDi { get; set; }
    }
}
