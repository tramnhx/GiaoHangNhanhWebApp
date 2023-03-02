using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class KhachHang : BaseEntity
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { get; set; }
        public ICollection<VanDon> VanDons { get; set; }
    }
}
