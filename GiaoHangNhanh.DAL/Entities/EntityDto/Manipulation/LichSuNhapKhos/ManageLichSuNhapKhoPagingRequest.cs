using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuNhapKhos
{
    public class ManageLichSuNhapKhoPagingRequest : PagingRequestBase
    {
        public int? FilterByBuuCucGuiHangId { get; set; }
        public int? FilterByUserId { get; set; }
    }
}
