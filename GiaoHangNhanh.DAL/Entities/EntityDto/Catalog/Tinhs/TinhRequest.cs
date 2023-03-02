using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs
{
    public class TinhRequest
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
        public bool IsDefault { set; get; }
    }
}
