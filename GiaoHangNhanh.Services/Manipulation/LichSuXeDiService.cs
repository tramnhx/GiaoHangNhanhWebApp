using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDis;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuXeDiService
    {
        //Task<ApiResult<List<LichSuXeDiDto>>> GetAll(ManageLichSuXeDiPagingRequest request);
        Task<PagedResult<LichSuXeDiDto>> GetManageListPaging(ManageLichSuXeDiPagingRequest request);
        Task<ApiResult<int>> CreateAsync(LichSuXeDiRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        //Task<ApiResult<LichSuXeDiDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(LichSuXeDiRequest request);
    }
    public class LichSuXeDiService : ILichSuXeDiService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public LichSuXeDiService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> CreateAsync(LichSuXeDiRequest request)
        {
            try
            {
                var lichSuXeDi = new LichSuChuyenHang()
                {
                    BuuCucId = request.MaTramTiepId,
                    VanDonId = request.VanDonId,
                    SealXe = request.SealXe,
                    MaSealBao = request.SealBao,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = request.ModifieDate,
                    IsXeDi = true,
                };
                _context.LichSuChuyenHangs.Add(lichSuXeDi);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(lichSuXeDi.Id);
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
                var lichSuXeDis = await _context.LichSuChuyenHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (lichSuXeDis == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in lichSuXeDis)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuChuyenHangs.UpdateRange(lichSuXeDis);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
        public async Task<PagedResult<LichSuXeDiDto>> GetManageListPaging(ManageLichSuXeDiPagingRequest request)
        {
            //1. Select join
            var query = from xd in _context.LichSuChuyenHangs
                        .Where(x => x.IsDeleted == false && x.IsXeDi == true)
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
            var data = await query.Select(x => new LichSuXeDiDto()
            {
                Id = x.xd.Id,
                SealXe = x.xd.SealXe,
                SealBao = x.xd.MaSealBao,
                CreatedDate = x.xd.CreatedDate,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    StrNgayGuiHang = x.v.NgayGuiHang.ToString("dd/MM/yyyy HH:mm:ss"),
                },
                BuuCuc = new BuuCucDto()
                {
                    Id = x.b.Id,
                    Code = x.b.Code,
                    Name = x.b.Name
                }
            }).AsNoTracking().ToListAsync();
            //4. Select and projection
            var pagedResult = new PagedResult<LichSuXeDiDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuXeDiRequest request)
        {
            try
            {
                var LichSuXeDi = await _context.LichSuChuyenHangs.FindAsync(request.Id);

                if (LichSuXeDi == null) throw new ApplicationException($"Không tìm thấy id: {request.Id}");

                LichSuXeDi.SealXe = request.SealXe;
                LichSuXeDi.MaSealBao = request.SealBao;
                LichSuXeDi.BuuCucId = request.MaTramTiepId;
                LichSuXeDi.VanDonId = request.VanDonId;
                LichSuXeDi.CreatedDate = DateTime.Now;
                _context.LichSuChuyenHangs.Update(LichSuXeDi);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
