using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhanLoaiHangBatThuongs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Catalog
{
    public interface IPhanLoaiHangBatThuongService
    {
        Task<ApiResult<List<PhanLoaiHangBatThuongDto>>> GetAll(ManagePhanLoaiHangBatThuongPagingRequest request);
        Task<PagedResult<PhanLoaiHangBatThuongDto>> GetManageListPaging(ManagePhanLoaiHangBatThuongPagingRequest request);
        Task<ApiResult<int>> CreateAsync(PhanLoaiHangBatThuongRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<PhanLoaiHangBatThuongDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(PhanLoaiHangBatThuongRequest request);
    }
    public class PhanLoaiHangBatThuongService : IPhanLoaiHangBatThuongService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public PhanLoaiHangBatThuongService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<PhanLoaiHangBatThuongDto>>> GetAll(ManagePhanLoaiHangBatThuongPagingRequest request)
        {
            var query = from p in _context.PhanLoaiHangBatThuongs
                        .Where(x => x.IsDeleted == false)
                        select new { p };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.p.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<PhanLoaiHangBatThuongDto>>(await query.Select(x => new PhanLoaiHangBatThuongDto()
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

        public async Task<PagedResult<PhanLoaiHangBatThuongDto>> GetManageListPaging(ManagePhanLoaiHangBatThuongPagingRequest request)
        {
            //1. Selep join
            var query = from p in _context.PhanLoaiHangBatThuongs
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
            var data = await query.Select(x => new PhanLoaiHangBatThuongDto()
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
            var pagedResult = new PagedResult<PhanLoaiHangBatThuongDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(PhanLoaiHangBatThuongRequest request)
        {
            try
            {
                var phanLoaiHangBatThuong = new PhanLoaiHangBatThuong()
                {
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.PhanLoaiHangBatThuongs.Add(phanLoaiHangBatThuong);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(phanLoaiHangBatThuong.Id);
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
                var phanLoaiHangBatThuong = await _context.PhanLoaiHangBatThuongs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (phanLoaiHangBatThuong == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in phanLoaiHangBatThuong)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.PhanLoaiHangBatThuongs.UpdateRange(phanLoaiHangBatThuong);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<PhanLoaiHangBatThuongDto>> GetById(int id)
        {
            var phanLoaiHangBatThuong = await _context.PhanLoaiHangBatThuongs.FindAsync(id);

            if (phanLoaiHangBatThuong.IsDeleted == true || phanLoaiHangBatThuong == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var phanLoaiHangBatThuongDto = new PhanLoaiHangBatThuongDto()
            {
                Id = phanLoaiHangBatThuong.Id,
                Name = phanLoaiHangBatThuong.Name,
                Code = phanLoaiHangBatThuong.Code,
                CreatedDate = phanLoaiHangBatThuong.CreatedDate,
                ModifiedDate = phanLoaiHangBatThuong.ModifiedDate,
                Description = phanLoaiHangBatThuong.Description,
                CreatedUserId = phanLoaiHangBatThuong.CreatedUserId,
                ModifiedUserId = phanLoaiHangBatThuong.ModifiedUserId,
                SortOrder = phanLoaiHangBatThuong.SortOrder,
                IsDeleted = phanLoaiHangBatThuong.IsDeleted
            };

            return new ApiSuccessResult<PhanLoaiHangBatThuongDto>(phanLoaiHangBatThuongDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(PhanLoaiHangBatThuongRequest request)
        {
            try
            {
                var phanLoaiHangBatThuong = await _context.PhanLoaiHangBatThuongs.FindAsync(request.Id);

                if (phanLoaiHangBatThuong.IsDeleted == true || phanLoaiHangBatThuong == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                phanLoaiHangBatThuong.Code = request.Code;
                phanLoaiHangBatThuong.Name = request.Name;
                phanLoaiHangBatThuong.SortOrder = request.SortOrder;
                phanLoaiHangBatThuong.Description = request.Description;
                phanLoaiHangBatThuong.ModifiedDate = DateTime.Now;
                phanLoaiHangBatThuong.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

    }
}
