using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs
{
    public class ManageKhuVucPagingRequest : PagingRequestBase
    {
        public int? FilterByHuyenId { set; get; }
        public int? FilterByTinhId { set; get; }
    }
}
