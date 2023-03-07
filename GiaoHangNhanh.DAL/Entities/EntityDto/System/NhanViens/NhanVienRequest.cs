using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens
{
    public class NhanVienRequest
    {
        public int? Id { set; get; }
        public string Code { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { set; get; }
        public int? BuuCucLamViecId { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string NgaySinh { get; set; }
        public string NgayLamViec { get; set; }
        public string NgayNghiViec { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public string NoiSinh { get; set; }
        public int? GenderId { get; set; }
        public bool IsActive { get; set; }
    }
}
