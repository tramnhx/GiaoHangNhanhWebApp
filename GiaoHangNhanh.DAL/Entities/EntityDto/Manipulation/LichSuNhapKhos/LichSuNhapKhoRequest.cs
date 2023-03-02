using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuNhapKhos
{
    public class LichSuNhapKhoRequest
    {
        public int? Id { get; set; }
        public int BuuCucGuiHangId { get; set; }
        public Guid AppUser { set; get; }
        public int VanDonId { get; set; }

        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
