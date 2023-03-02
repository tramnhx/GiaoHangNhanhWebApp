using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuDongBaos;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuDongBaoService
    {
        //Task<ApiResult<List<LichSuDongBaoDto>>> GetAll(LichSuDongBaoRequest request);
        //Task<ApiResult<LichSuDongBaoDto>> GetById(int id);
        Task<PagedResult<LichSuDongBaoDto>> GetManageListPaging(ManageLichSuDongBaoPagingRequest request);
        Task<ApiResult<int>> CreateAsync(LichSuDongBaoRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<int>> UpdateAsync(LichSuDongBaoRequest request);
    }
    public class LichSuDongBaoService : ILichSuDongBaoService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public LichSuDongBaoService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> CreateAsync(LichSuDongBaoRequest request)
        {
            try
            {
                var LichSuDongBao = new LichSuBaoHang()
                {
                    VanDonId = request.VanDonId,
                    SealBao = request.SealBao,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = request.CreatedUserId,
                    IsDongBao = true
                };
                _context.LichSuBaoHangs.Add(LichSuDongBao);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(LichSuDongBao.Id);
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
                var LichSuDongBaos = await _context.LichSuBaoHangs.Where(m => request.Ids.Contains(m.Id) && m.IsDongBao == true).AsNoTracking().ToListAsync();
                if (LichSuDongBaos == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in LichSuDongBaos)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuBaoHangs.UpdateRange(LichSuDongBaos);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<LichSuDongBaoDto>> GetManageListPaging(ManageLichSuDongBaoPagingRequest request)
        {
            //1. Selebh join
            var query = from bh in _context.LichSuBaoHangs
                        .Where(x => x.IsDeleted == false && x.IsDongBao == true)
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
            var data = await query.Select(x => new LichSuDongBaoDto()
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
            var pagedResult = new PagedResult<LichSuDongBaoDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuDongBaoRequest request)
        {
            try
            {
                var LichSuDongBao = await _context.LichSuBaoHangs.FindAsync(request.Id);

                if (LichSuDongBao == null) throw new GiaoHangNhanhException($"Không tìm thấy bao đóng có id: {request.Id}");

                if (LichSuDongBao.IsDongBao == true)
                {
                    LichSuDongBao.VanDonId = request.VanDonId;
                    LichSuDongBao.SealBao = request.SealBao;
                    LichSuDongBao.CreatedDate = DateTime.Now;
                    LichSuDongBao.ModifiedUserId = request.ModifiedUserId;

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
