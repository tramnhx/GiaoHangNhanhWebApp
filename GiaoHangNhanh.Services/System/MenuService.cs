using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.System
{
    public interface IMenuService
    {
        Task<ApiResult<List<MenuDto>>> GetAll(ManageMenuPagingRequest request);
        Task<PagedResult<MenuDto>> GetManageListPaging(ManageMenuPagingRequest request);
        Task<ApiResult<int>> CreateAsync(MenuRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<MenuDto>> GetById(MenuRequest request);
        Task<ApiResult<int>> UpdateAsync(MenuRequest request);
    }
    public class MenuService : IMenuService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public MenuService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<List<MenuDto>>> GetAll(ManageMenuPagingRequest request)
        {
            //1. Select join
            var query = from m in _context.Menus
                        select new { m };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.m.Name.Contains(request.TextSearch));

            return new ApiSuccessResult<List<MenuDto>>(await query.Select(x => new MenuDto()
            {
                Id = x.m.Id,
                Code = x.m.Code,
                Name = x.m.Name,
                ParentId = x.m.ParentId,
                Icon = x.m.Icon,
                CreatedDate = x.m.CreatedDate,
                Description = x.m.Description,
                Link = x.m.Link
            }).AsNoTracking().ToListAsync());
        }
        public async Task<ApiResult<int>> CreateAsync(MenuRequest request)
        {
            try
            {
                var menu = new Menu()
                {
                    SortOrder = request.SortOrder,
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    ParentId = request.ParentId,
                    Icon = request.Icon,
                    IsActive = true,
                    ControllerName = request.ControllerName,
                    ActionName = request.ActionName,
                    Link = request.ControllerName.Trim() != null && request.ActionName.Trim() != null ? $"/{request.ControllerName}/{request.ActionName}" : null
                };

                _context.Menus.Add(menu);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(menu.Id);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            try
            {
                var menus = await _context.Menus.Where(m => request.Ids.Contains(m.Id)).ToListAsync();
                if (menus == null) throw new GiaoHangNhanhException($"Cannot find store: {request.Ids.ToString()}");

                _context.Menus.RemoveRange(menus);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<MenuDto>> GetManageListPaging(ManageMenuPagingRequest request)
        {
            //1. Select join
            var query = from m in _context.Menus
                        select new { m };
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.m.Name.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new MenuDto()
            {
                Id = x.m.Id,
                SortOrder = x.m.SortOrder,
                Code = x.m.Code,
                Icon = x.m.Icon,
                Name = x.m.Name,
                ParentId = x.m.ParentId,
                ControllerName = x.m.ControllerName,
                ActionName = x.m.ActionName,
                Description = x.m.Description
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<MenuDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<MenuDto>> GetById(MenuRequest request)
        {
            var menu = await _context.Menus.FindAsync(request.Id);

            var menuDto = new MenuDto()
            {
                Id = menu.Id,
                CreatedDate = menu.CreatedDate,
                Description = menu.Description,
                SortOrder = menu.SortOrder,
                Code = menu.Code,
                Name = menu.Name,
                ParentId = menu.ParentId,
                Icon = menu.Icon,
                Link = menu.Link
            };

            return new ApiSuccessResult<MenuDto>(menuDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(MenuRequest request)
        {
            try
            {
                var menu = await _context.Menus.FindAsync(request.Id);

                if (menu == null) throw new GiaoHangNhanhException($"Cannot find a menu with id: {request.Id}");

                menu.Name = request.Name;
                menu.Description = request.Description;
                menu.SortOrder = request.SortOrder;
                menu.Code = request.Code;
                menu.ControllerName = request.ControllerName;
                menu.ActionName = request.ActionName;
                menu.ParentId = request.ParentId;
                menu.Icon = request.Icon;
                menu.ModifiedDate = DateTime.Now;
                if (request.ControllerName != null && request.ActionName != null)
                {
                    menu.Link = $"/{request.ControllerName}/{request.ActionName}";
                }
                

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
