using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuPhatHangs
{
    public class LichSuPhatHangRequest : BaseEntity
    {
        public int? Id { get; set; }
        public int VanDonId { get; set; }
        public int NhanVienId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifieDate { get; set; }
        public string CreatedUserId { set; get; }
        public string ModifiedUserId { set; get; }
    }
}
