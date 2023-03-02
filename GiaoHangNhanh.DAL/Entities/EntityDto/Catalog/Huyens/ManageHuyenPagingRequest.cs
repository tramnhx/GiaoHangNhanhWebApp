using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens
{
    public class ManageHuyenPagingRequest : PagingRequestBase
    {
        public int? FilterByTinhId { get; set; }
    }
}
