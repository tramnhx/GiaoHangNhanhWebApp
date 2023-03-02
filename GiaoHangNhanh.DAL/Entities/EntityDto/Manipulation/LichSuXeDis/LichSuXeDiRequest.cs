using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDis
{
    public class LichSuXeDiRequest
    {
        public int? Id { get; set; }
        public int VanDonId { get; set; }
        public int MaTramTiepId { get; set; }

        public string SealXe { get; set; }
        public string SealBao { get; set; }

        public DateTime CreatedDate;
        public DateTime ModifieDate;

        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}