using GiaoHangNhanh.DAL.Entities.EntityDto.Common;


namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DuyetChuyenHoans
{
    public class ManageDuyetChuyenHoanPagingRequest : PagingRequestBase
    {
        public int? FilterByDangKyChuyenHoanId { set; get; }
        public int? FilterByVanDonId { set; get; }


    }
}
