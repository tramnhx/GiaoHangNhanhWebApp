using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuChuyenHangs;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuChuyenHangService
    {
        Task<ApiResult<int>> CreateXeDenXeDiAsync(LichSuChuyenHangRequest request, string sealXe);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<LichSuChuyenHangDto>> GetById(int id);
        Task<PagedResult<LichSuChuyenHangDto>> GetManageListPaging(ManageLichSuChuyenHangPagingRequest request);
        Task<ApiResult<int>> UpdateAsync(LichSuChuyenHangRequest request);

    }
    public class LichSuChuyenHangService : ILichSuChuyenHangService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public LichSuChuyenHangService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> CreateXeDenXeDiAsync(LichSuChuyenHangRequest request, string sealXe)
        {
            try
            {
                var chuyenHang = new LichSuChuyenHang()
                {
                    SealXe = sealXe,
                    BuuCucId = request.BuuCucId,
                    IsXeDi = request.IsXeDi != null ? request.IsXeDi : false,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedUserId = request.CreatedUserId

                };
                _context.LichSuChuyenHangs.Add(chuyenHang);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(chuyenHang.Id);
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
                var chuyenHangs = await _context.LichSuChuyenHangs.Where(m => request.Ids.Contains(m.Id)).ToListAsync();
                if (chuyenHangs == null) throw new GiaoHangNhanhException($"Không tìm thấy Id: {string.Join(";", request.Ids)}");

                foreach (var item in chuyenHangs)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuChuyenHangs.UpdateRange(chuyenHangs);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public Task<ApiResult<LichSuChuyenHangDto>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<LichSuChuyenHangDto>> GetManageListPaging(ManageLichSuChuyenHangPagingRequest request)
        {
            //1. Select join
            var query = from l in _context.LichSuChuyenHangs
                        join b in _context.BuuCucs on l.BuuCucId equals b.Id into l_b
                        from b in l_b.DefaultIfEmpty()
                        select new { l, b };
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.l.SealXe.Contains(request.TextSearch));

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

            var data = await query.Select(x => new LichSuChuyenHangDto()
            {
                Id = x.l.Id,
                SealXe = x.l.SealXe,
                IsXeDi = x.l.IsXeDi == true ? "Xe đi" : "Xe đến",
                TenBuuCuc = x.b.Name,
                StrCreatedDay = x.l.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"),
                IsDeleted = x.l.IsDeleted,
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<LichSuChuyenHangDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuChuyenHangRequest request)
        {
            try
            {
                var chuyenHang = await _context.LichSuChuyenHangs.FindAsync(request.Id);

                if (chuyenHang.IsDeleted == true || chuyenHang == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                chuyenHang.BuuCucId = request.BuuCucId;
                chuyenHang.ModifiedDate = DateTime.Now;
                chuyenHang.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
