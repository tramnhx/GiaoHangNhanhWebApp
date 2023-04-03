using GiaoHangNhanh.DAL.Entities.Entity;
using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class LichSuBaoHang : BaseEntity
    {
        public int VanDonId { get; set; }
        public int? BaoHangId { get; set; }
        public VanDon VanDon { get; set; }
        public BaoHang BaoHang { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsTrongBao { get; set; }
    }
}
