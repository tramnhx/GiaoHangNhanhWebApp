using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class LichSuChuyenHang : BaseEntity
    {
        public int? BuuCucId { get; set; }
        public string SealXe { get; set; }
        public int? VanDonId { get; set; }
        public string MaSealBao { get; set; }
        public bool IsXeDi { get; set; }
    }
}
