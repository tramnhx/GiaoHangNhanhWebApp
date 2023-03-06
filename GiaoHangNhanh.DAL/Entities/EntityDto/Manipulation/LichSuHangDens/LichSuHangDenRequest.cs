using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuHangDens
{
    public class LichSuHangDenRequest
    {
        public int? Id { get; set; }
        public List<int?> VanDonIds { get; set; }       
        public int BuuCucId { get; set; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public string CreatedUserId { set; get; }
        public string ModifiedUserId { set; get; }
    }
}
