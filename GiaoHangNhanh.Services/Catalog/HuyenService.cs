using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
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
    public interface IHuyenService
    {
        Task<ApiResult<List<HuyenDto>>> GetAll(ManageHuyenPagingRequest request);
        Task<PagedResult<HuyenDto>> GetManageListPaging(ManageHuyenPagingRequest request);
        Task<ApiResult<int>> CreateAsync(HuyenRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<HuyenDto>> GetById(int TinhId);
        Task<ApiResult<int>> UpdateAsync(HuyenRequest request);
    }
    public class HuyenService : IHuyenService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public HuyenService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<HuyenDto>>> GetAll(ManageHuyenPagingRequest request)
        {
            var query = from h in _context.Huyens
                        .Where(x => x.IsDeleted == false)
                        join t in _context.Tinhs on h.TinhId equals t.Id
                        select new { h, t };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.h.Name.Contains(request.TextSearch));
            }
                
            return new ApiSuccessResult<List<HuyenDto>>(await query.Select(x => new HuyenDto()
            {
                Id = x.h.Id,
                Name = x.h.Name,
                Code = x.h.Code,
                CreatedDate = x.h.CreatedDate,
                ModifiedDate = x.h.ModifiedDate,
                Description = x.h.Description,
                CreatedUserId = x.h.CreatedUserId,
                ModifiedUserId = x.h.ModifiedUserId,
                SortOrder = x.h.SortOrder,
                IsDeleted = x.h.IsDeleted,
                Tinh = new TinhDto()
                {
                    Id = x.t.Id,
                    Name = x.t.Name
                }
            }).AsNoTracking().ToListAsync());
        }

        public async Task<PagedResult<HuyenDto>> GetManageListPaging(ManageHuyenPagingRequest request)
        {
            //1. Seleh join
            var query = from h in _context.Huyens
                        .Where(x => x.IsDeleted == false)
                        join t in _context.Tinhs on h.TinhId equals t.Id
                        select new { h, t };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.h.Name.Contains(request.TextSearch));

            if (request.FilterByTinhId != null)
            {
                query = query.Where(x => x.t.Id == request.FilterByTinhId.Value);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new HuyenDto()
            {
                Id = x.h.Id,
                Name = x.h.Name,
                Code = x.h.Code,
                CreatedDate = x.h.CreatedDate,
                ModifiedDate = x.h.ModifiedDate,
                Description = x.h.Description,
                CreatedUserId = x.h.CreatedUserId,
                ModifiedUserId = x.h.ModifiedUserId,
                SortOrder = x.h.SortOrder,
                IsDeleted = x.h.IsDeleted,
                Tinh = new TinhDto()
                {
                    Id = x.t.Id,
                    Name = x.t.Name
                }
            }).AsNoTracking().ToListAsync();

            //4. Seleh and projehion
            var pagedResult = new PagedResult<HuyenDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(HuyenRequest request)
        {
            try
            {
                var huyen = new Huyen()
                {
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    TinhId = request.TinhId,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.Huyens.Add(huyen);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(huyen.Id);
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
                var huyen = await _context.Huyens.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (huyen == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in huyen)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.Huyens.UpdateRange(huyen);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<HuyenDto>> GetById(int id)
        {
            var huyen = await _context.Huyens.FindAsync(id);

            if (huyen.IsDeleted == true || huyen == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var HuyenDto = new HuyenDto()
            {
                Id = huyen.Id,
                Name = huyen.Name,
                Code = huyen.Code,
                CreatedDate = huyen.CreatedDate,
                ModifiedDate = huyen.ModifiedDate,
                Description = huyen.Description,
                CreatedUserId = huyen.CreatedUserId,
                ModifiedUserId = huyen.ModifiedUserId,
                SortOrder = huyen.SortOrder,
                IsDeleted = huyen.IsDeleted,
                Tinh = new TinhDto()
                {
                    Id = huyen.Tinh.Id,
                    Name = huyen.Tinh.Name
                }
            };

            return new ApiSuccessResult<HuyenDto>(HuyenDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(HuyenRequest request)
        {
            try
            {
                var huyen = await _context.Huyens.FindAsync(request.Id);

                if (huyen.IsDeleted == true || huyen == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                huyen.Code = request.Code;
                huyen.Name = request.Name;
                huyen.TinhId = request.TinhId;
                huyen.SortOrder = request.SortOrder;
                huyen.Description = request.Description;
                huyen.ModifiedDate = DateTime.Now;
                huyen.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}