using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class NhanVien : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { set; get; }
        public int? BuuCucLamViecId { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public BuuCuc BuuCuc { get; set; }
        public DateTime? NgaySinh { get; set; }
        public DateTime? NgayLamViec { get; set; }
        public DateTime? NgayNghiViec { get; set; }
        public string NoiSinh { get; set; }
        public int? GenderId { get; set; }
        public bool IsActive { get; set; }
        public Gender Gender { get; set; }
    }
}
