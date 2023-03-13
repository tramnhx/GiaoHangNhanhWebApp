using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans
{
    public class KyNhanRequest
    {
        public int? Id { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public bool IsDeleted { set; get; }
        public int BuuCucId { get; set; }
        public int VanDonId { get; set; }
        public Guid NhanVienPhat { get; set; }
        public string TenNguoiKy { get; set; }
        public bool DauKyThay { get; set; }
        public DateTime NgayKyNhan { get; set; }
    }
}
