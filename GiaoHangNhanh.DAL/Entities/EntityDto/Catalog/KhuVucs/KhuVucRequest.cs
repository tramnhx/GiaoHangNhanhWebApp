using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs
{
    public class KhuVucRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Description { get; set; }
        public int SortOrder { set; get; }
        public bool IsDefault { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public int HuyenId { get; set; }
    }
}
