using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs
{
    public class LichSuBaoHangRequest
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public bool IsDongBao { get; set; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public IList<int> VanDonIds { get; set; }
    }
}
