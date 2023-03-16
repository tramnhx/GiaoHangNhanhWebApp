using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuPhatHangs
{
    public class ManageLichSuPhatHangPagingRequest : PagingRequestBase
    {
        public int? FilterByNhanVienId { set; get; }
        public int? FilterByStaffId { get; set; }
    }
}
