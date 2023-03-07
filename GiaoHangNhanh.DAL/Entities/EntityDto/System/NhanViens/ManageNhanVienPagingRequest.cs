using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens
{
    public class ManageNhanVienPagingRequest : PagingRequestBase
    {
        public int? FilterByBuuCucLamViecId { set; get; }
        public int? FilterByTinhId { set; get; }
        public int? FilterByHuyenId { set; get; }
        public int? FilterByKhuVucId { set; get; }
        public int? FilterByGenderId { set; get; }
    }
}
