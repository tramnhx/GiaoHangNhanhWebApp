using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class CongTyGuiHang : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public int? TinhId { get; set; }
        public int? HuyenId { get; set; }
        public int? KhuVucId { get; set; }
        public string DiaChi { get; set; }
        public Tinh Tinh { get; set; }
        public Huyen Huyen { get; set; }
        public KhuVuc KhuVuc { get; set; }
        public ICollection<VanDon> VanDons { get; set; }
    }
}
