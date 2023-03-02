using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Users
{
    public class ManageUserPagingRequest : PagingRequestBase
    {
        public string AppUserId { set; get; }
        public string FromDate { set; get; }
        public string ToDate { set; get; }
    }
}