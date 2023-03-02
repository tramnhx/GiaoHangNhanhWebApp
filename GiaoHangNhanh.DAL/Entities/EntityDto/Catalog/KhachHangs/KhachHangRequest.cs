using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhachHangs
{
    public class KhachHangRequest
    {
        public int? Id { set; get; }
        public int SortOrder { set; get; }
        public virtual string Code { set; get; }
        public virtual string Name { set; get; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { get; set; }
        public virtual string Description { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public bool IsDeleted { set; get; }
    }
}
