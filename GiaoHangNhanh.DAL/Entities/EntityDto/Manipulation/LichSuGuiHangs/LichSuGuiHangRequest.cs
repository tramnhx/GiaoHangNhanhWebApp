using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuGuiHangs
{
    public class LichSuGuiHangRequest
    {
        public int? Id { get; set; }
        public int BuuCucId { get; set; }
        public int VanDonId { get; set; }
        public DateTime CreatDate { get; set; }
        public string KhuVucHangDenId { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
