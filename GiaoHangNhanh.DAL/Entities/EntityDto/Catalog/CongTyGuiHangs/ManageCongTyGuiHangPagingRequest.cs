using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs
{
    public class ManageCongTyGuiHangPagingRequest : PagingRequestBase
    {
        public int? FilterByTinhId { set; get; }
        public int? FilterByHuyenId { set; get; }
        public int? FilterByKhuVucId { set; get; }
    }
}
