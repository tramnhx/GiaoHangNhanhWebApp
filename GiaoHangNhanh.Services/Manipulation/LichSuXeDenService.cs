using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDens;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuXeDenService
    {
        Task<PagedResult<LichSuXeDenDto>> GetManageListPaging(ManageLichSuXeDenPagingRequest request);
        Task<ApiResult<int>> CreateAsync(LichSuXeDenRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<int>> UpdateAsync(LichSuXeDenRequest request);
    }
    public class LichSuXeDenService : ILichSuXeDenService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public LichSuXeDenService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> CreateAsync(LichSuXeDenRequest request)
        {
            try
            {
                var lichSuXeDen = new LichSuChuyenHang()
                {
                    BuuCucId = request.TramTruocId,
                    SealXe = request.SealXe,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    CreatedDate = DateTime.Now,
                    VanDonId = request.VanDonId,
                    IsXeDi = false,
                    MaSealBao = request.SealBao,
                };
                _context.LichSuChuyenHangs.Add(lichSuXeDen);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(lichSuXeDen.Id);
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
                var lichSuXeDens = await _context.LichSuChuyenHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (lichSuXeDens == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in lichSuXeDens)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuChuyenHangs.UpdateRange(lichSuXeDens);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<LichSuXeDenDto>> GetManageListPaging(ManageLichSuXeDenPagingRequest request)
        {
            //1. Select join
            var query = from xd in _context.LichSuChuyenHangs
                        .Where(x => x.IsXeDi == false && x.IsDeleted == false)
                        join v in _context.VanDons on xd.VanDonId equals v.Id
                        join b in _context.BuuCucs on xd.BuuCucId equals b.Id
                        select new { xd, v, b };

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
            var data = await query.Select(x => new LichSuXeDenDto()
            {
                Id = x.xd.Id,
                SealXe = x.xd.SealXe,
                SealBao = x.xd.MaSealBao,
                CreatedDate = x.xd.CreatedDate,
                Description = x.xd.Description,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                },
                BuuCuc = new BuuCucDto()
                {
                    Id = x.b.Id,
                    Code = x.b.Code,
                    Name = x.b.Name
                }
            }).AsNoTracking().ToListAsync();
            //4. Select and projection
            var pagedResult = new PagedResult<LichSuXeDenDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuXeDenRequest request)
        {
            try
            {
                var LichSuXeDen = await _context.LichSuChuyenHangs.FindAsync(request.Id);

                if (LichSuXeDen == null) throw new ApplicationException($"Không tìm thấy id: {request.Id}");

                LichSuXeDen.SealXe = request.SealXe;
                LichSuXeDen.MaSealBao = request.SealBao;
                LichSuXeDen.BuuCucId = request.TramTruocId;
                LichSuXeDen.VanDonId = request.VanDonId;
                LichSuXeDen.CreatedDate = DateTime.Now;
                _context.LichSuChuyenHangs.Update(LichSuXeDen);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
