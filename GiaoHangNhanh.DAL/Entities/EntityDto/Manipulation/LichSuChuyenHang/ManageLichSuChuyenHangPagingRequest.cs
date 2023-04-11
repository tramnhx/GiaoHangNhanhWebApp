using GiaoHangNhanh.DAL.Entities.EntityDto.Common;


namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuChuyenHangs
{
    public class ManageLichSuChuyenHangPagingRequest : PagingRequestBase
    {
        public bool? IsXeDi { get; set; }
        public int? FilterByBuuCucId { set; get; }
        public int? FilterByIsXeDi { set; get; }
    }
}
