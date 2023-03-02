using GiaoHangNhanh.DAL.Entities.EntityDto.Enums;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using System.Collections.Generic;

namespace GiaoHangNhanh.AdminApp
{
    public class InternalService
    {
        public static UserDto FixedUserRole(UserDto currentUserRole, string controllerName, string actionName)
        {
            var result = new UserDto();
            List<MenuAppRoleDto> menuAppRoles = new List<MenuAppRoleDto>();
            menuAppRoles.Add(new MenuAppRoleDto() { MenuAppRoleType = (int)MenuAppRoleType.SystemDataView, IsAllow = true });
            menuAppRoles.Add(new MenuAppRoleDto() { MenuAppRoleType = (int)MenuAppRoleType.SystemDataEdit, IsAllow = true });
            menuAppRoles.Add(new MenuAppRoleDto() { MenuAppRoleType = (int)MenuAppRoleType.SystemDataDelete, IsAllow = true });
            menuAppRoles.Add(new MenuAppRoleDto() { MenuAppRoleType = (int)MenuAppRoleType.DownloadExcel, IsAllow = true });
            result.MenuAppRoles = menuAppRoles;
            result.IsAllowView = true;
            result.IsAllowEdit = true;
            result.IsAllowDelete = true;
            result.Id = string.Empty;

            return result;
        }
    }
}
