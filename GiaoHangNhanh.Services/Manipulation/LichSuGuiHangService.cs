using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuGuiHangs;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuGuiHangService
    {
        
        Task<PagedResult<LichSuGuiHangDto>> GetManageListPaging(ManageLichSuGuiHangPagingRequest request);
        Task<ApiResult<int>> CreateAsync(LichSuGuiHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);      
        Task<ApiResult<int>> UpdateAsync(LichSuGuiHangRequest request);
    }
    public class LichSuGuiHangService : ILichSuGuiHangService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public LichSuGuiHangService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async  Task<ApiResult<int>> CreateAsync(LichSuGuiHangRequest request)
        {
            try
            {
                var lichSuGuiHang = new LichSuGuiHang()
                {
                    TramSauId = request.BuuCucId,
                    VanDonId = request.VanDonId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,

                };
                _context.LichSuGuiHangs.Add(lichSuGuiHang);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(lichSuGuiHang.Id);
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
                var lichSuGuiHangs = await _context.LichSuGuiHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (lichSuGuiHangs == null) throw new GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

                foreach (var item in lichSuGuiHangs)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuGuiHangs.UpdateRange(lichSuGuiHangs);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<LichSuGuiHangDto>> GetManageListPaging(ManageLichSuGuiHangPagingRequest request)
        {
            //1. Select join
            var query = from gh in _context.LichSuGuiHangs
                        .Where(x => x.IsDeleted == false)
                        join v in _context.VanDons on gh.VanDonId equals v.Id
                        join b in _context.BuuCucs on gh.TramSauId equals b.Id
                        select new { gh,v,b };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.b.Name.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new LichSuGuiHangDto()
            {
                Id = x.gh.Id,
                CreatedDate = x.gh.CreatedDate,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    Name = x.v.Name,
                    BuuCuc = new BuuCucDto()
                    {
                        Id= x.v.BuuCuc.Id,
                        Code = x.v.BuuCuc.Code,
                    }
                },
                BuuCuc = new BuuCucDto()
                {
                    Id = x.b.Id,
                    Code = x.b.Code,
                    Name = x.b.Name,
                }
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<LichSuGuiHangDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuGuiHangRequest request)
        {
            try
            {
                var LichSuGuiHang = await _context.LichSuGuiHangs.FindAsync(request.Id);

                if (LichSuGuiHang == null) throw new ApplicationException($"Không tìm thấy id: {request.Id}");

                LichSuGuiHang.VanDonId = request.VanDonId;
                LichSuGuiHang.TramSauId = request.BuuCucId;
                _context.LichSuGuiHangs.Update(LichSuGuiHang);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
