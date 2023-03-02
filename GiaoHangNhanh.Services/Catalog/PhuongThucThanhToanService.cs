using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhuongThucThanhToans;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Catalog
{
    public interface IPhuongThucThanhToanService
    {
        Task<ApiResult<List<PhuongThucThanhToanDto>>> GetAll(ManagePhuongThucThanhToanPagingRequest request);
        Task<PagedResult<PhuongThucThanhToanDto>> GetManageListPaging(ManagePhuongThucThanhToanPagingRequest request);
        Task<ApiResult<int>> CreateAsync(PhuongThucThanhToanRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<PhuongThucThanhToanDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(PhuongThucThanhToanRequest request);
    }
    public class PhuongThucThanhToanService : IPhuongThucThanhToanService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public PhuongThucThanhToanService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<PhuongThucThanhToanDto>>> GetAll(ManagePhuongThucThanhToanPagingRequest request)
        {
            var query = from p in _context.PhuongThucThanhToans
                        .Where(x => x.IsDeleted == false)
                        select new { p };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.p.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<PhuongThucThanhToanDto>>(await query.Select(x => new PhuongThucThanhToanDto()
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

        public async Task<PagedResult<PhuongThucThanhToanDto>> GetManageListPaging(ManagePhuongThucThanhToanPagingRequest request)
        {
            //1. Selep join
            var query = from p in _context.PhuongThucThanhToans
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
            var data = await query.Select(x => new PhuongThucThanhToanDto()
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
            var pagedResult = new PagedResult<PhuongThucThanhToanDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(PhuongThucThanhToanRequest request)
        {
            try
            {
                var phuongThucThanhToan = new PhuongThucThanhToan()
                {
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.PhuongThucThanhToans.Add(phuongThucThanhToan);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(phuongThucThanhToan.Id);
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
                var phuongThucThanhToan = await _context.PhuongThucThanhToans.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (phuongThucThanhToan == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in phuongThucThanhToan)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.PhuongThucThanhToans.UpdateRange(phuongThucThanhToan);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<PhuongThucThanhToanDto>> GetById(int id)
        {
            var phuongThucThanhToan = await _context.PhuongThucThanhToans.FindAsync(id);

            if (phuongThucThanhToan.IsDeleted == true || phuongThucThanhToan == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var phuongThucThanhToanDto = new PhuongThucThanhToanDto()
            {
                Id = phuongThucThanhToan.Id,
                Name = phuongThucThanhToan.Name,
                Code = phuongThucThanhToan.Code,
                CreatedDate = phuongThucThanhToan.CreatedDate,
                ModifiedDate = phuongThucThanhToan.ModifiedDate,
                Description = phuongThucThanhToan.Description,
                CreatedUserId = phuongThucThanhToan.CreatedUserId,
                ModifiedUserId = phuongThucThanhToan.ModifiedUserId,
                SortOrder = phuongThucThanhToan.SortOrder,
                IsDeleted = phuongThucThanhToan.IsDeleted
            };

            return new ApiSuccessResult<PhuongThucThanhToanDto>(phuongThucThanhToanDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(PhuongThucThanhToanRequest request)
        {
            try
            {
                var phuongThucThanhToan = await _context.PhuongThucThanhToans.FindAsync(request.Id);

                if (phuongThucThanhToan.IsDeleted == true || phuongThucThanhToan == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                phuongThucThanhToan.Code = request.Code;
                phuongThucThanhToan.Name = request.Name;
                phuongThucThanhToan.SortOrder = request.SortOrder;
                phuongThucThanhToan.Description = request.Description;
                phuongThucThanhToan.ModifiedDate = DateTime.Now;
                phuongThucThanhToan.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

    }
}
