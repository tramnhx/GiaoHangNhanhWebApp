using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
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
    public interface IBuuCucService
    {
        Task<ApiResult<List<BuuCucDto>>> GetAll(ManageBuuCucPagingRequest request);
        Task<PagedResult<BuuCucDto>> GetManageListPaging(ManageBuuCucPagingRequest request);
        Task<ApiResult<int>> CreateAsync(BuuCucRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<BuuCucDto>> GetById(int id);
        Task<ApiResult<BuuCucDto>> GetByKhuVucId(int khuVucId);
        Task<ApiResult<int>> UpdateAsync(BuuCucRequest request);
    }
    public class BuuCucService : IBuuCucService

    {
        private readonly GiaoHangNhanhDbContext _context;

        public BuuCucService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<BuuCucDto>>> GetAll(ManageBuuCucPagingRequest request)
        {
            var query = from bc in _context.BuuCucs
                        .Where(x => x.IsDeleted == false)
                        join t in _context.Tinhs on bc.TinhId equals t.Id
                        join h in _context.Huyens on bc.HuyenId equals h.Id
                        join kv in _context.KhuVucs on bc.KhuVucId equals kv.Id
                        select new { bc, t, h, kv };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.bc.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<BuuCucDto>>(await query.Select(x => new BuuCucDto()
            {
                Id = x.bc.Id,
                Name = x.bc.Name,
                Code = x.bc.Code,
                CreatedDate = x.bc.CreatedDate,
                ModifiedDate = x.bc.ModifiedDate,
                Description = x.bc.Description,
                CreatedUserId = x.bc.CreatedUserId,
                ModifiedUserId = x.bc.ModifiedUserId,
                SortOrder = x.bc.SortOrder,
                IsDeleted = x.bc.IsDeleted,
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

        public async Task<PagedResult<BuuCucDto>> GetManageListPaging(ManageBuuCucPagingRequest request)
        {
            //1. Selebc join
            var query = from bc in _context.BuuCucs
                        .Where(x => x.IsDeleted == false)
                        join t in _context.Tinhs on bc.TinhId equals t.Id
                        join h in _context.Huyens on bc.HuyenId equals h.Id
                        join kv in _context.KhuVucs on bc.KhuVucId equals kv.Id
                        select new { bc, t, h, kv };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.bc.Name.Contains(request.TextSearch));

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
            var data = await query.Select(x => new BuuCucDto()
            {
                Id = x.bc.Id,
                Name = x.bc.Name,
                DiaChi = $"{x.kv.Name}, {x.h.Name}, { x.t.Name }",
                Code = x.bc.Code,
                CreatedDate = x.bc.CreatedDate,
                ModifiedDate = x.bc.ModifiedDate,
                Description = x.bc.Description,
                CreatedUserId = x.bc.CreatedUserId,
                ModifiedUserId = x.bc.ModifiedUserId,
                SortOrder = x.bc.SortOrder,
                IsDeleted = x.bc.IsDeleted,
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

            //4. Selebc and projebcion
            var pagedResult = new PagedResult<BuuCucDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> CreateAsync(BuuCucRequest request)
        {
            try
            {
                var buuCuc = new BuuCuc()
                {
                    Code = request.Code,
                    Name = request.Name,
                    TinhId = request.TinhId,
                    HuyenId = request.HuyenId,
                    KhuVucId = request.KhuVucId,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    SortOrder = request.SortOrder,
                };
                _context.BuuCucs.Add(buuCuc);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(buuCuc.Id);
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
                var buuCuc = await _context.BuuCucs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (buuCuc == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in buuCuc)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.BuuCucs.UpdateRange(buuCuc);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<BuuCucDto>> GetById(int id)
        {
            var buuCuc = await _context.BuuCucs.FindAsync(id);

            if (buuCuc.IsDeleted == true || buuCuc == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var buuCucDto = new BuuCucDto()
            {
                Id = buuCuc.Id,
                Name = buuCuc.Name,
                Code = buuCuc.Code,
                CreatedDate = buuCuc.CreatedDate,
                ModifiedDate = buuCuc.ModifiedDate,
                Description = buuCuc.Description,
                CreatedUserId = buuCuc.CreatedUserId,
                ModifiedUserId = buuCuc.ModifiedUserId,
                SortOrder = buuCuc.SortOrder,
                IsDeleted = buuCuc.IsDeleted,
                KhuVuc = new KhuVucDto()
                {
                    Id = buuCuc.KhuVuc.Id,
                    Name = buuCuc.KhuVuc.Name
                },
                Huyen = new HuyenDto()
                {
                    Id = buuCuc.Huyen.Id,
                    Name = buuCuc.Huyen.Name
                },
                Tinh = new TinhDto()
                {
                    Id = buuCuc.Tinh.Id,
                    Name = buuCuc.Tinh.Name
                }
            };

            return new ApiSuccessResult<BuuCucDto>(buuCucDto);
        }

        public async Task<ApiResult<BuuCucDto>> GetByKhuVucId(int khuVucId)
        {
            var buuCuc = await (from b in _context.BuuCucs
                                //where v.Id == id && v.IsDeleted == false
                                .Where(x => x.KhuVucId == khuVucId && x.IsDeleted == false)
                                select new BuuCucDto()
                                {
                                    Id = b.Id,
                                    Name = b.Name,
                                    Code = b.Code,
                                    CreatedDate = b.CreatedDate,
                                    ModifiedDate = b.ModifiedDate,
                                    Description = b.Description,
                                    CreatedUserId = b.CreatedUserId,
                                    ModifiedUserId = b.ModifiedUserId,
                                    SortOrder = b.SortOrder,
                                    IsDeleted = b.IsDeleted,
                                    KhuVuc = new KhuVucDto()
                                    {
                                        Id = b.KhuVuc.Id,
                                        Name = b.KhuVuc.Name
                                    },
                                    Huyen = new HuyenDto()
                                    {
                                        Id = b.Huyen.Id,
                                        Name = b.Huyen.Name
                                    },
                                    Tinh = new TinhDto()
                                    {
                                        Id = b.Tinh.Id,
                                        Name = b.Tinh.Name
                                    }
                                }).AsNoTracking().FirstOrDefaultAsync();

            //if (buuCuc.IsDeleted == true || buuCuc == null) throw new GiaoHangNhanhException($"Không tìm thấy bưu cục với khu vực id: {khuVucId}");

            return new ApiSuccessResult<BuuCucDto>(buuCuc);
        }
        public async Task<ApiResult<int>> UpdateAsync(BuuCucRequest request)
        {
            try
            {
                var buuCuc = await _context.BuuCucs.FindAsync(request.Id);

                if (buuCuc.IsDeleted == true || buuCuc == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                buuCuc.Code = request.Code;
                buuCuc.Name = request.Name;
                buuCuc.TinhId = request.TinhId;
                buuCuc.HuyenId = request.HuyenId;
                buuCuc.KhuVucId = request.KhuVucId;
                buuCuc.SortOrder = request.SortOrder;
                buuCuc.Description = request.Description;
                buuCuc.ModifiedDate = DateTime.Now;
                buuCuc.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
