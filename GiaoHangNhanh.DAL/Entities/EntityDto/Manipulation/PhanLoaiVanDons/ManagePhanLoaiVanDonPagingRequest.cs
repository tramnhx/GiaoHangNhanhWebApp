using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.PhanLoaiVanDons
{
    public class ManagePhanLoaiVanDonPagingRequest : PagingRequestBase
    {
        public int? FilterByDMPhanLoaiHangBatThuong { set; get; }
    }
}
