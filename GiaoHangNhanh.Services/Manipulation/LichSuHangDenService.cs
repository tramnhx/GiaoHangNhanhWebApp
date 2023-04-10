using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuHangDens;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuHangDenService
    {
        Task<ApiResult<int>> CreateWithVanDonAsync(LichSuHangDenRequest request, int? vanDonId);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<LichSuHangDenDto>> GetById(int id);
        Task<PagedResult<LichSuHangDenDto>> GetManageListPaging(ManageLichSuHangDenPagingRequest request);
    }
    public class LichSuHangDenService : ILichSuHangDenService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public LichSuHangDenService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> CreateWithVanDonAsync(LichSuHangDenRequest request, int? vanDonId)
        {
            try
            {
                var hangDen = new LichSuHangDen()
                {
                    BuuCucId = request.BuuCucId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(request.CreatedUserId),
                    VanDonId = vanDonId
                };
                _context.LichSuHangDens.Add(hangDen);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(hangDen.Id);
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
                var hangDens = await _context.LichSuHangDens.Where(m => request.Ids.Contains(m.Id)).ToListAsync();
                if (hangDens == null) throw new GiaoHangNhanhException($"Cannot find Id: {string.Join(";", request.Ids)}");

                foreach (var item in hangDens)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuHangDens.UpdateRange(hangDens);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public Task<ApiResult<LichSuHangDenDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<LichSuHangDenDto>> GetManageListPaging(ManageLichSuHangDenPagingRequest request)
        {
            try
            {
                //1. Select join
                var query = from l in _context.LichSuHangDens
                            where l.IsDeleted == false
                            join b in _context.BuuCucs on l.BuuCucId equals b.Id
                            join v in _context.VanDons on l.VanDonId equals v.Id
                            select new { l, b, v };

                //2. filter
                if (!string.IsNullOrEmpty(request.TextSearch))
                    query = query.Where(x => x.l.VanDon.Code.Contains(request.TextSearch));

                if (request.FilterByBuuCucId != null)
                {
                    query = query.Where(x => x.b.Id == request.FilterByBuuCucId.Value);
                }

                //3. Paging
                int totalRow = await query.CountAsync();

                if (request.PageIndex != null)
                {
                    query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                                .Take(request.PageSize);
                }

                var data = await query.Select(x => new LichSuHangDenDto()
                {
                    Id = x.l.Id,
                    StrCreatedDate = x.l.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"),
                    BuuCuc = new BuuCucDto()
                    {
                        Id = x.b.Id,
                        Name = x.b.Name,
                        Code = x.b.Code
                    },
                    VanDon = new VanDonDto()
                    {
                        Id = x.v.Id,
                        Code = x.v.Code,
                        StrNgayGuiHang = x.v.NgayGuiHang.ToString("dd/MM/yyyy HH:mm:ss"),
                    }
                }).AsNoTracking().ToListAsync();

                //4. Select and projection
                var pagedResult = new PagedResult<LichSuHangDenDto>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                };
                return pagedResult;

            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}
