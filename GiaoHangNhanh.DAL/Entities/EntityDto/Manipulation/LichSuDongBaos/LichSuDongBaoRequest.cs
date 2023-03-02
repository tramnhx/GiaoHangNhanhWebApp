using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuDongBaos
{
    public class LichSuDongBaoRequest
    {
        public int? Id { get; set; }
        public int VanDonId { get; set; }
        public string SealBao { get; set; } 
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedIdDate { get; set; }     
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
