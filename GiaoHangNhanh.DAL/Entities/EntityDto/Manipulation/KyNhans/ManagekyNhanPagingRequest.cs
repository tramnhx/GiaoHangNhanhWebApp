using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans
{
    public class ManageKyNhanPagingRequest : PagingRequestBase
    {
        public int? FilterByDMBuuCuc { set; get; }
        public int? FilterByDMNhanVien { set; get; }
    }
}
