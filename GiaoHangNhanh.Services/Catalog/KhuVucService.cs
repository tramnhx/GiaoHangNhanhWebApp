using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
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
    public interface IKhuVucService
    {
        Task<ApiResult<List<KhuVucDto>>> GetAll(ManageKhuVucPagingRequest request);
        Task<PagedResult<KhuVucDto>> GetManageListPaging(ManageKhuVucPagingRequest request);
        Task<ApiResult<int>> CreateAsync(KhuVucRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<KhuVucDto>> GetById(int IdHuyen);
        Task<ApiResult<int>> UpdateAsync(KhuVucRequest request);
    }

    public class KhuVucService : IKhuVucService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public KhuVucService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<KhuVucDto>>> GetAll(ManageKhuVucPagingRequest request)
        {
            var query = from kv in _context.KhuVucs
                        .Where(x => x.IsDeleted == false)
                        join h in _context.Huyens on kv.HuyenId equals h.Id
                        join t in _context.Tinhs on h.TinhId equals t.Id
                        select new { kv, t, h };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.kv.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<KhuVucDto>>(await query.Select(x => new KhuVucDto()
            {
                Id = x.kv.Id,
                Name = x.kv.Name,
                Code = x.kv.Code,
                CreatedDate = x.kv.CreatedDate,
                ModifiedDate = x.kv.ModifiedDate,
                Description = x.kv.Description,
                CreatedUserId = x.kv.CreatedUserId,
                ModifiedUserId = x.kv.ModifiedUserId,
                SortOrder = x.kv.SortOrder,
                IsDeleted = x.kv.IsDeleted,
                Huyen = new HuyenDto()
                {
                    Id = x.h.Id,
                    Name = x.h.Name
                },
                Tinh = new TinhDto()
                {
                    Id = x.t.Id,
                    Name = x.t.Name
                }
            }).AsNoTracking().ToListAsync());
        }

        public async Task<PagedResult<KhuVucDto>> GetManageListPaging(ManageKhuVucPagingRequest request)
        {
            //1. Selekv join
            var query = from kv in _context.KhuVucs
                        .Where(x => x.IsDeleted == false)
                        join h in _context.Huyens on kv.HuyenId equals h.Id
                        join t in _context.Tinhs on h.TinhId equals t.Id
                        select new { kv, t, h };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.kv.Name.Contains(request.TextSearch));

            if (request.FilterByTinhId != null)
            {
                query = query.Where(x => x.t.Id == request.FilterByTinhId.Value);
            }

            if (request.FilterByHuyenId != null)
            {
                query = query.Where(x => x.h.Id == request.FilterByHuyenId.Value);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new KhuVucDto()
            {
                Id = x.kv.Id,
                Name = x.kv.Name,
                Code = x.kv.Code,
                CreatedDate = x.kv.CreatedDate,
                ModifiedDate = x.kv.ModifiedDate,
                Description = x.kv.Description,
                CreatedUserId = x.kv.CreatedUserId,
                ModifiedUserId = x.kv.ModifiedUserId,
                SortOrder = x.kv.SortOrder,
                IsDeleted = x.kv.IsDeleted,
                Huyen = new HuyenDto()
                {
                    Id = x.h.Id,
                    Name = x.h.Name
                },
                Tinh = new TinhDto()
                {
                    Id = x.t.Id,
                    Name = x.t.Name
                }
            }).AsNoTracking().ToListAsync();

            //4. Selekv and projekvion
            var pagedResult = new PagedResult<KhuVucDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(KhuVucRequest request)
        {
            try
            {
                var khuVuc = new KhuVuc()
                {
                    Code = request.Code,
                    Name = request.Name,
                    HuyenId = request.HuyenId,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.KhuVucs.Add(khuVuc);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(khuVuc.Id);
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
                var khuVuc = await _context.KhuVucs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (khuVuc == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in khuVuc)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.KhuVucs.UpdateRange(khuVuc);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<KhuVucDto>> GetById(int id)
        {
            var khuVuc = await _context.KhuVucs.FindAsync(id);

            if (khuVuc.IsDeleted == true || khuVuc == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var KhuVucDto = new KhuVucDto()
            {
                Id = khuVuc.Id,
                Name = khuVuc.Name,
                Code = khuVuc.Code,
                CreatedDate = khuVuc.CreatedDate,
                ModifiedDate = khuVuc.ModifiedDate,
                Description = khuVuc.Description,
                CreatedUserId = khuVuc.CreatedUserId,
                ModifiedUserId = khuVuc.ModifiedUserId,
                SortOrder = khuVuc.SortOrder,
                IsDeleted = khuVuc.IsDeleted,
                Huyen = new HuyenDto()
                {
                    Id = khuVuc.Huyen.Id,
                    Name = khuVuc.Huyen.Name
                },
                Tinh = new TinhDto()
                {
                    Id = khuVuc.Huyen.Tinh.Id,
                    Name = khuVuc.Huyen.Tinh.Name
                }
            };

            return new ApiSuccessResult<KhuVucDto>(KhuVucDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(KhuVucRequest request)
        {
            try
            {
                var khuVuc = await _context.KhuVucs.FindAsync(request.Id);

                if (khuVuc.IsDeleted == true || khuVuc == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                khuVuc.Code = request.Code;
                khuVuc.Name = request.Name;
                khuVuc.HuyenId = request.HuyenId;
                khuVuc.SortOrder = request.SortOrder;
                khuVuc.Description = request.Description;
                khuVuc.ModifiedDate = DateTime.Now;
                khuVuc.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
