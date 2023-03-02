using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDis
{
    public class ManageLichSuXeDiPagingRequest : PagingRequestBase
    {
        public int? FilterByDMBuuCuc { set; get; }
        public int? FilterByVanDon { set; get; }
    }
}
