using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs
{
    public class CongTyGuiHangRequest
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { set; get; }
        public string PhoneNumber { get; set; }
        public int TinhId { get; set; }
        public int HuyenId { get; set; }
        public int KhuVucId { get; set; }
        public string DiaChi { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Description { get; set; }
        public int SortOrder { set; get; }
        public bool IsDefault { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
