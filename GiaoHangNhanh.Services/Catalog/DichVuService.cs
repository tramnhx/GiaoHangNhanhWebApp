using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Catalog
{
    public interface IDichVuService
    {
        Task<ApiResult<List<DichVuDto>>> GetAll(ManageDichVuPagingRequest request);
        Task<PagedResult<DichVuDto>> GetManageListPaging(ManageDichVuPagingRequest request);
        Task<ApiResult<int>> CreateAsync(DichVuRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<DichVuDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(DichVuRequest request);
    }
    public class DichVuService : IDichVuService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public DichVuService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<DichVuDto>>> GetAll(ManageDichVuPagingRequest request)
        {
            var query = from dv in _context.DichVus
                        .Where(x => x.IsDeleted == false)
                        select new {dv};

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.dv.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<DichVuDto>>(await query.Select(x => new DichVuDto()
            {
                Id = x.dv.Id,
                Name = x.dv.Name,
                Code = x.dv.Code,
                CreatedDate = x.dv.CreatedDate,
                ModifiedDate = x.dv.ModifiedDate,
                Description = x.dv.Description,
                CreatedUserId = x.dv.CreatedUserId,
                ModifiedUserId = x.dv.ModifiedUserId,
                SortOrder = x.dv.SortOrder,
                IsDeleted = x.dv.IsDeleted
            }).AsNoTracking().ToListAsync());
        }

        public async Task<PagedResult<DichVuDto>> GetManageListPaging(ManageDichVuPagingRequest request)
        {
            //1. Seledv join
            var query = from dv in _context.DichVus
                        .Where(x => x.IsDeleted == false)
                        select new {dv};

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.dv.Name.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize).Take(request.PageSize);
            }
            var data = await query.Select(x => new DichVuDto()
            {
                Id = x.dv.Id,
                Name = x.dv.Name,
                Code = x.dv.Code,
                CreatedDate = x.dv.CreatedDate,
                ModifiedDate = x.dv.ModifiedDate,
                Description = x.dv.Description,
                CreatedUserId = x.dv.CreatedUserId,
                ModifiedUserId = x.dv.ModifiedUserId,
                SortOrder = x.dv.SortOrder,
                IsDeleted = x.dv.IsDeleted
            }).AsNoTracking().ToListAsync();

            //4. Seledv and projedvion
            var pagedResult = new PagedResult<DichVuDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(DichVuRequest request)
        {
            try
            {
                var dichVu = new DichVu()
                {
                    Code = request.Code,
                    Name = request.Name,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.DichVus.Add(dichVu);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(dichVu.Id);
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
                var dichVu = await _context.DichVus.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (dichVu == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in dichVu)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.DichVus.UpdateRange(dichVu);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<DichVuDto>> GetById(int id)
        {
            var dichVu = await _context.DichVus.FindAsync(id);

            if (dichVu.IsDeleted == true || dichVu == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var DichVuDto = new DichVuDto()
            {
                Id = dichVu.Id,
                Name = dichVu.Name,
                Code = dichVu.Code,
                CreatedDate = dichVu.CreatedDate,
                ModifiedDate = dichVu.ModifiedDate,
                Description = dichVu.Description,
                CreatedUserId = dichVu.CreatedUserId,
                ModifiedUserId = dichVu.ModifiedUserId,
                SortOrder = dichVu.SortOrder,
                IsDeleted = dichVu.IsDeleted
            };

            return new ApiSuccessResult<DichVuDto>(DichVuDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(DichVuRequest request)
        {
            try
            {
                var dichVu = await _context.DichVus.FindAsync(request.Id);

                if (dichVu.IsDeleted == true || dichVu == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                dichVu.Code = request.Code;
                dichVu.Name = request.Name;
                dichVu.SortOrder = request.SortOrder;
                dichVu.Description = request.Description;
                dichVu.ModifiedDate = DateTime.Now;
                dichVu.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

    }
}
