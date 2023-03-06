using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuHangDens
{
    public class ManageLichSuHangDenPagingRequest : PagingRequestBase
    {
        public int? FilterByBuuCucId { set; get; }
    }
}
