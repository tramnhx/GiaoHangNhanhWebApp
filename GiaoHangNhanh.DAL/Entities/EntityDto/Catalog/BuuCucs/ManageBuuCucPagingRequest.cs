using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs
{
    public class ManageBuuCucPagingRequest : PagingRequestBase
    {
        public int? FilterByTinhId { set; get; }
        public int? FilterByHuyenId { set; get; }
        public int? FilterByKhuVucId { set; get; }
    }
}
