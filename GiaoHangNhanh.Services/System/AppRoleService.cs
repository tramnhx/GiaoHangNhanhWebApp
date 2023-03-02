using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.System
{
    public interface IAppRoleService
    {
        Task<ApiResult<List<AppRoleDto>>> GetAll(ManageAppRolePagingRequest request);
        Task<PagedResult<AppRoleDto>> GetManageListPaging(ManageAppRolePagingRequest request);
        Task<ApiResult<string>> CreateAsync(AppRoleRequest request);
        Task<ApiResult<int>> DeleteByIds(AppRoleDeleteRequest request);
        Task<ApiResult<AppRoleDto>> GetById(string id);
        Task<ApiResult<string>> UpdateAsync(AppRoleRequest request);
    }
    public class AppRoleService : IAppRoleService
    {
        private readonly GiaoHangNhanhDbContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        public AppRoleService(GiaoHangNhanhDbContext context,
            RoleManager<AppRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<ApiResult<string>> CreateAsync(AppRoleRequest request)
        {
            var appRole = new AppRole()
            {
                ChucDanh = request.Name,
                Name = request.Code,
                Description = request.Description,
                NgayTao = request.NgayTao,
                CreatedUserId = request.CreatedUserId,
            };

            _context.Roles.Add(appRole);
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<string>(appRole.Id.ToString());
        }

        public async Task<ApiResult<int>> DeleteByIds(AppRoleDeleteRequest request)
        {
            try
            {
                var roles = await _context.Roles.Where(m => request.Ids.Contains(m.Id.ToString())).ToListAsync();
                if (roles == null) throw new GiaoHangNhanhException($"Cannot find Id: {String.Join(";", request.Ids)}");
                foreach (var item in roles)
                {
                    item.IsDelete = true;
                    item.ModifiedUserId = request.UserDelete;
                    item.NgaySua = DateTime.Now;
                    _context.Update(item);
                }

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiResult<int>()
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResult<List<AppRoleDto>>> GetAll(ManageAppRolePagingRequest request)
        {
            var query = from r in _context.Roles
                        .Where(x => x.IsDelete == false)
                        select new { r };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.r.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<AppRoleDto>>(await query.Select(x => new AppRoleDto()
            {
                Id = x.r.Id.ToString(),
                Code = x.r.Name,
                Name = x.r.ChucDanh,
                Description = x.r.Description
            }).AsNoTracking().ToListAsync());
        }

        public async Task<ApiResult<AppRoleDto>> GetById(string id)
        {
            var appRoleIdValue = Guid.Parse(id);
            var role = await _context.Roles.FindAsync(appRoleIdValue);

            var roleDto = new AppRoleDto()
            {
                Id = role.Id.ToString(),
                ChucDanh = role.ChucDanh,
                Name = role.Name,
                IsDelete = role.IsDelete,
                Description = role.Description,
                NgaySua = role.NgaySua,
                NgayTao = role.NgayTao,
                CreatedUserId = role.CreatedUserId,
                ModifiedUserId = role.ModifiedUserId

            };

            return new ApiSuccessResult<AppRoleDto>(roleDto);
        }

        public async Task<PagedResult<AppRoleDto>> GetManageListPaging(ManageAppRolePagingRequest request)
        {
            //1. Select join
            var query = from r in _context.Roles
                        .Where(x => x.IsDelete == false)
                        select new { r };
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.r.Name.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new AppRoleDto()
            {
                Id = x.r.Id.ToString(),
                Code = x.r.Name,
                Name = x.r.ChucDanh,
                Description = x.r.Description
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<AppRoleDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<string>> UpdateAsync(AppRoleRequest request)
        {
            var appRole = await _roleManager.FindByIdAsync(request.Id);

            if (appRole == null) throw new GiaoHangNhanhException($"Cannot find a role with id: {request.Id}");

            appRole.ChucDanh = request.Name;
            appRole.Description = request.Description;
            appRole.Name = request.Code;
            appRole.ModifiedUserId = request.ModifiedUserId;
            appRole.NgaySua = request.NgaySua;

            await _roleManager.UpdateAsync(appRole);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<string>(request.Id);
        }
    }
}
