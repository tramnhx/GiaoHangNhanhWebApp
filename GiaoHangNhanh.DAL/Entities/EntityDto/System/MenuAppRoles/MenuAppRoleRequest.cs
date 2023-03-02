using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles
{
    public class MenuAppRoleRequest
    {
        public string AppRoleId { set; get; }
        public int MenuId { set; get; }
        public int MenuAppRoleType { set; get; }
        public bool IsAllow { set; get; }
        public Guid UserRoleId { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}