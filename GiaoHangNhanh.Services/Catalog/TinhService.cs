using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Catalog
{
    public interface ITinhService
    {
        Task<ApiResult<List<TinhDto>>> GetAll(ManageTinhPagingRequest request);
        Task<PagedResult<TinhDto>> GetManageListPaging(ManageTinhPagingRequest request);
        Task<ApiResult<int>> CreateAsync(TinhRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<TinhDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(TinhRequest request);
    }
    public class TinhService : ITinhService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public TinhService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<TinhDto>>> GetAll(ManageTinhPagingRequest request)
        {
            var query = from p in _context.Tinhs
                        .Where(x => x.IsDeleted == false)
                        select new { p };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.p.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<TinhDto>>(await query.Select(x => new TinhDto()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                Code = x.p.Code,
                CreatedDate = x.p.CreatedDate,
                ModifiedDate = x.p.ModifiedDate,
                Description = x.p.Description,
                CreatedUserId = x.p.CreatedUserId,
                ModifiedUserId = x.p.ModifiedUserId,
                SortOrder = x.p.SortOrder,
                IsDeleted = x.p.IsDeleted
            }).AsNoTracking().ToListAsync());
        }

        public async Task<PagedResult<TinhDto>> GetManageListPaging(ManageTinhPagingRequest request)
        {
            //1. Selep join
            var query = from p in _context.Tinhs
                        .Where(x => x.IsDeleted == false)
                        select new { p };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.p.Name.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new TinhDto()
            {
                Id = x.p.Id,
                Name = x.p.Name,
                Code = x.p.Code,
                CreatedDate = x.p.CreatedDate,
                ModifiedDate = x.p.ModifiedDate,
                Description = x.p.Description,
                CreatedUserId = x.p.CreatedUserId,
                ModifiedUserId = x.p.ModifiedUserId,
                SortOrder = x.p.SortOrder,
                IsDeleted = x.p.IsDeleted
            }).AsNoTracking().ToListAsync();

            //4. Selep and projepion
            var pagedResult = new PagedResult<TinhDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(TinhRequest request)
        {
            try
            {
                var tinh = new Tinh()
                {
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.Tinhs.Add(tinh);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(tinh.Id);
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
                var tinhs = await _context.Tinhs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (tinhs == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in tinhs)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.Tinhs.UpdateRange(tinhs);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<TinhDto>> GetById(int id)
        {
            var tinh = await _context.Tinhs.FindAsync(id);

            if (tinh.IsDeleted == true || tinh == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var tinhDto = new TinhDto()
            {
                Id = tinh.Id,
                Name = tinh.Name,
                Code = tinh.Code,
                CreatedDate = tinh.CreatedDate,
                ModifiedDate = tinh.ModifiedDate,
                Description = tinh.Description,
                CreatedUserId = tinh.CreatedUserId,
                ModifiedUserId = tinh.ModifiedUserId,
                SortOrder = tinh.SortOrder,
                IsDeleted = tinh.IsDeleted
            };

            return new ApiSuccessResult<TinhDto>(tinhDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(TinhRequest request)
        {
            try
            {
                var tinh = await _context.Tinhs.FindAsync(request.Id);

                if (tinh.IsDeleted == true || tinh == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                tinh.Code = request.Code;
                tinh.Name = request.Name;
                tinh.SortOrder = request.SortOrder;
                tinh.Description = request.Description;
                tinh.ModifiedDate = DateTime.Now;
                tinh.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

    }
}
