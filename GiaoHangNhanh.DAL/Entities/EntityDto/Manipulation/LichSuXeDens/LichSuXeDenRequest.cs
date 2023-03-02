using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDens
{
    public class LichSuXeDenRequest
    {
        public int? Id { get; set; }
        public int VanDonId { get; set; }
        public int TramTruocId { get; set; }
        public string Description { get; set; }
        public string SealXe { get; set; }
        public string SealBao { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifieDate { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
