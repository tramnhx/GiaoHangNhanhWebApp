using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhachHangs
{
    public class KhachHangDto : BaseDto
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { get; set; }
    }
}
