using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles
{
    public class ManageMenuAppRolePagingRequest : PagingRequestBase
    {
        public string AppRoleId { set; get; }
        public int? MenuId { set; get; }
    }
}
