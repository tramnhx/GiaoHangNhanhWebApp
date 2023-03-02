using GiaoHangNhanh.DAL.Entities.Entity;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans
{
    public class KyNhanRequest
    {
        public int? Id { set; get; }
        public int SortOrder { set; get; }
        public virtual string Code { set; get; }
        public virtual string Name { set; get; }
        public virtual string Description { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public bool IsDeleted { set; get; }
        public int BuuCucId { get; set; }
        public int NhanVienCPId { get; set; }
        public int VanDonId { get; set; }
        public string TenNguoiKy { get; set; }
        public bool DauKyThay { get; set; }
        public BuuCuc DanhMucBuuCuc { get; set; }
        public NhanVienChuyenPhat NhanVienChuyenPhat { get; set; }
        public VanDon VanDon { get; set; }
    }
}
