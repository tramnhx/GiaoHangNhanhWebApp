using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Enums;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.System
{
    public interface IMenuAppRoleService
    {
        Task<ApiResult<int>> Save(MenuAppRoleRequest request);
        Task<ApiResult<List<MenuAppRoleDto>>> GetAll(ManageMenuAppRolePagingRequest request);
    }
    public class MenuAppRoleService : IMenuAppRoleService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public MenuAppRoleService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<MenuAppRoleDto>>> GetAll(ManageMenuAppRolePagingRequest request)
        {
            var roleId = Guid.Parse(request.AppRoleId);
            var query = from m in _context.MenuAppRoles
                        where m.AppRoleId == roleId
                        select new { m };
            return new ApiSuccessResult<List<MenuAppRoleDto>>(await query.Select(x => new MenuAppRoleDto()
            {
                Id = x.m.Id,
                MenuId = x.m.MenuId,
                AppRoleId = x.m.AppRoleId.ToString(),
                MenuAppRoleType = (int)x.m.MenuAppRoleType,
                IsAllow = x.m.IsAllow
            }).AsNoTracking().ToListAsync());
        }

        public async Task<ApiResult<int>> Save(MenuAppRoleRequest request)
        {
            Guid roleId = Guid.Parse(request.AppRoleId);
            MenuAppRoleType menuType = (MenuAppRoleType)request.MenuAppRoleType;
            var oldMenuAppRole = await _context.MenuAppRoles.Where(x => x.AppRoleId == roleId && x.MenuAppRoleType == menuType && x.MenuId == request.MenuId).ToListAsync();
            if (oldMenuAppRole.Count == 0)
            {
                var menuApprole = new MenuAppRole()
                {
                    AppRoleId = Guid.Parse(request.AppRoleId),
                    MenuId = request.MenuId,
                    MenuAppRoleType = menuType,
                    IsAllow = true
                };
                _context.MenuAppRoles.Add(menuApprole);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(menuApprole.Id);
            }
            else
            {
                _context.MenuAppRoles.RemoveRange(oldMenuAppRole);
                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            

        }
    }
}
