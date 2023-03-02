using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.PhanLoaiVanDons
{
    public class PhanLoaiVanDonRequest
    {
        public int? Id { get; set; }
        public string MaVanDon { get; set; }
        public int IdDMPhanLoaiHangBT { get; set; }
        public string Ten { get; set; }
        public string Ma { set; get; }
        public DateTime NgayTao { get; set; }
        public DateTime NgaySua { get; set; }
        public string GhiChu { get; set; }
        public int SortOrder { set; get; }
        public bool IsDeleted { set; get; }
        public bool IsDefault { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
