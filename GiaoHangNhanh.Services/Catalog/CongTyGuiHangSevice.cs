using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
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
    public interface ICongTyGuiHangService
    {
        Task<ApiResult<List<CongTyGuiHangDto>>> GetAll(ManageCongTyGuiHangPagingRequest request);
        Task<PagedResult<CongTyGuiHangDto>> GetManageListPaging(ManageCongTyGuiHangPagingRequest request);
        Task<ApiResult<int>> CreateAsync(CongTyGuiHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<CongTyGuiHangDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(CongTyGuiHangRequest request);
    }
    public class CongTyGuiHangService : ICongTyGuiHangService

    {
        private readonly GiaoHangNhanhDbContext _context;

        public CongTyGuiHangService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<CongTyGuiHangDto>>> GetAll(ManageCongTyGuiHangPagingRequest request)
        {
            var query = from ct in _context.CongTyGuiHangs
                        .Where(x => x.IsDeleted == false)
                        join t in _context.Tinhs on ct.TinhId equals t.Id
                        join h in _context.Huyens on ct.HuyenId equals h.Id
                        join kv in _context.KhuVucs on ct.KhuVucId equals kv.Id
                        select new { ct, t, h, kv };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.ct.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<CongTyGuiHangDto>>(await query.Select(x => new CongTyGuiHangDto()
            {
                Id = x.ct.Id,
                Name = x.ct.Name,
                PhoneNumber = x.ct.PhoneNumber,
                DiaChi = x.ct.DiaChi,
                Code = x.ct.Code,
                CreatedDate = x.ct.CreatedDate,
                ModifiedDate = x.ct.ModifiedDate,
                Description = x.ct.Description,
                CreatedUserId = x.ct.CreatedUserId,
                ModifiedUserId = x.ct.ModifiedUserId,
                SortOrder = x.ct.SortOrder,
                IsDeleted = x.ct.IsDeleted,
                KhuVuc = new KhuVucDto()
                {
                    Id = x.kv.Id,
                    Name = x.kv.Name
                },
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

        public async Task<PagedResult<CongTyGuiHangDto>> GetManageListPaging(ManageCongTyGuiHangPagingRequest request)
        {
            //1. Select join
            var query = from ct in _context.CongTyGuiHangs
                        .Where(x => x.IsDeleted == false)
                        join t in _context.Tinhs on ct.TinhId equals t.Id
                        join h in _context.Huyens on ct.HuyenId equals h.Id
                        join kv in _context.KhuVucs on ct.KhuVucId equals kv.Id
                        select new { ct, t, h, kv };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.ct.Name.Contains(request.TextSearch));

            if (request.FilterByTinhId != null)
            {
                query = query.Where(x => x.t.Id == request.FilterByTinhId.Value);
            }

            if (request.FilterByHuyenId != null)
            {
                query = query.Where(x => x.h.Id == request.FilterByHuyenId.Value);
            }

            if (request.FilterByKhuVucId != null)
            {
                query = query.Where(x => x.kv.Id == request.FilterByKhuVucId.Value);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new CongTyGuiHangDto()
            {
                Id = x.ct.Id,
                Name = x.ct.Name,
                PhoneNumber = x.ct.PhoneNumber,
                DiaChi = $"{x.t.Name}, {x.h.Name}, {x.kv.Name}",
                Code = x.ct.Code,
                CreatedDate = x.ct.CreatedDate,
                ModifiedDate = x.ct.ModifiedDate,
                Description = x.ct.Description,
                CreatedUserId = x.ct.CreatedUserId,
                ModifiedUserId = x.ct.ModifiedUserId,
                SortOrder = x.ct.SortOrder,
                IsDeleted = x.ct.IsDeleted,
                KhuVuc = new KhuVucDto()
                {
                    Id = x.kv.Id,
                    Name = x.kv.Name
                },
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

            //4. Select and projection
            var pagedResult = new PagedResult<CongTyGuiHangDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(CongTyGuiHangRequest request)
        {
            try
            {
                var congTy = new CongTyGuiHang()
                {
                    Code = request.Code,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    TinhId = request.TinhId,
                    HuyenId = request.HuyenId,
                    KhuVucId = request.KhuVucId,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.CongTyGuiHangs.Add(congTy);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(congTy.Id);
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
                var congTy = await _context.CongTyGuiHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (congTy == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in congTy)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.CongTyGuiHangs.UpdateRange(congTy);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<CongTyGuiHangDto>> GetById(int id)
        {
            var congTy = await _context.CongTyGuiHangs.FindAsync(id);

            if (congTy.IsDeleted == true || congTy == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var congTyDto = new CongTyGuiHangDto()
            {
                Id = congTy.Id,
                Name = congTy.Name,
                PhoneNumber = congTy.PhoneNumber,
                DiaChi = congTy.DiaChi,
                Code = congTy.Code,
                CreatedDate = congTy.CreatedDate,
                ModifiedDate = congTy.ModifiedDate,
                Description = congTy.Description,
                CreatedUserId = congTy.CreatedUserId,
                ModifiedUserId = congTy.ModifiedUserId,
                SortOrder = congTy.SortOrder,
                IsDeleted = congTy.IsDeleted,
                KhuVuc = new KhuVucDto()
                {
                    Id = congTy.KhuVuc.Id,
                    Name = congTy.KhuVuc.Name
                },
                Huyen = new HuyenDto()
                {
                    Id = congTy.Huyen.Id,
                    Name = congTy.Huyen.Name
                },
                Tinh = new TinhDto()
                {
                    Id = congTy.Tinh.Id,
                    Name = congTy.Tinh.Name
                }
            };

            return new ApiSuccessResult<CongTyGuiHangDto>(congTyDto);
        }

        public async Task<ApiResult<int>> UpdateAsync(CongTyGuiHangRequest request)
        {
            try
            {
                var congTy = await _context.CongTyGuiHangs.FindAsync(request.Id);

                if (congTy.IsDeleted == true || congTy == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                congTy.Code = request.Code;
                congTy.Name = request.Name;
                congTy.PhoneNumber = request.PhoneNumber;
                congTy.DiaChi = request.DiaChi;
                congTy.TinhId = request.TinhId;
                congTy.HuyenId = request.HuyenId;
                congTy.KhuVucId = request.KhuVucId;
                congTy.SortOrder = request.SortOrder;
                congTy.Description = request.Description;
                congTy.ModifiedDate = DateTime.Now;
                congTy.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
