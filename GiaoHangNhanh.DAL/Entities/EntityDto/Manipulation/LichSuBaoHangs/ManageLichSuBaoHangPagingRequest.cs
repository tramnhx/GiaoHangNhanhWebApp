using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs
{
    public class ManageLichSuBaoHangPagingRequest : PagingRequestBase
    {
        public string FilterByMaSealBao { get; set; }
    }
}
