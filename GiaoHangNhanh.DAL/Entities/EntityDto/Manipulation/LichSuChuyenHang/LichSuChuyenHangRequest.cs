using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuChuyenHangs
{
    public class LichSuChuyenHangRequest
    {
        public int? Id { get; set; }
        public bool IsXeDi { get; set; }
        public List<string> SealXes { get; set; }
        public int BuuCucId { get; set; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
