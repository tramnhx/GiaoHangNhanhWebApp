using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuThaoBaos;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuThaoBaoService
    {
        //Task<ApiResult<List<LichSuThaoBaoDto>>> GetAll(LichSuThaoBaoRequest request);
        //Task<ApiResult<LichSuThaoBaoDto>> GetById(int id);
        Task<PagedResult<LichSuThaoBaoDto>> GetManageListPaging(ManageLichSuThaoBaoPagingRequest request);
        Task<ApiResult<int>> CreateAsync(LichSuThaoBaoRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<int>> UpdateAsync(LichSuThaoBaoRequest request);
    }
    public class LichSuThaoBaoService : ILichSuThaoBaoService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public LichSuThaoBaoService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> CreateAsync(LichSuThaoBaoRequest request)
        {
            try
            {
                var LichSuThaoBao = new LichSuBaoHang()
                {
                    VanDonId = request.VanDonId,
                    SealBao = request.SealBao,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = request.CreatedUserId,
                    IsDongBao = false
                };
                _context.LichSuBaoHangs.Add(LichSuThaoBao);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(LichSuThaoBao.Id);
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
                var LichSuThaoBaos = await _context.LichSuBaoHangs.Where(m => request.Ids.Contains(m.Id) && m.IsDongBao == false).AsNoTracking().ToListAsync();
                if (LichSuThaoBaos == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in LichSuThaoBaos)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuBaoHangs.UpdateRange(LichSuThaoBaos);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<LichSuThaoBaoDto>> GetManageListPaging(ManageLichSuThaoBaoPagingRequest request)
        {
            //1. Selebh join
            var query = from bh in _context.LichSuBaoHangs
                        .Where(x => x.IsDeleted == false && x.IsDongBao == false)
                        join v in _context.VanDons on bh.VanDonId equals v.Id
                        select new { bh, v };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.bh.SealBao.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize).Take(request.PageSize);
            }
            var data = await query.Select(x => new LichSuThaoBaoDto()
            {
                Id = x.bh.Id,
                SealBao = x.bh.SealBao,
                CreatedDate = x.bh.CreatedDate,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    Name = x.v.Name,
                    NoiDungHangHoa = x.v.NoiDungHangHoa,
                    DichVu = new DichVuDto()
                    {
                        Id = x.v.DichVu.Id,
                        Name = x.v.DichVu.Name,
                    }
                },
            }).AsNoTracking().ToListAsync();

            //4. Selebh and projebhion
            var pagedResult = new PagedResult<LichSuThaoBaoDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuThaoBaoRequest request)
        {
            try
            {
                var LichSuThaoBao = await _context.LichSuBaoHangs.FindAsync(request.Id);

                if (LichSuThaoBao == null) throw new GiaoHangNhanhException($"Không tìm thấy bao đóng có id: {request.Id}");

                if (LichSuThaoBao.IsDongBao == false)
                {
                    LichSuThaoBao.VanDonId = request.VanDonId;
                    LichSuThaoBao.SealBao = request.SealBao;
                    LichSuThaoBao.CreatedDate = DateTime.Now;
                    LichSuThaoBao.ModifiedUserId = request.ModifiedUserId;

                    return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
                }
                else
                    throw new GiaoHangNhanhException($"Bao hàng có id: {request.Id} là bao đã tháo");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
