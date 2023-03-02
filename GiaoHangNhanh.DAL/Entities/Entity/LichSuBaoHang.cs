using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class LichSuBaoHang : BaseEntity
    {
        public string SealBao { get; set; }
        public bool IsDongBao { get; set; }
        public int? VanDonId { get; set; }
    }
}
