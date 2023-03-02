using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface IKyNhanService
    {
        Task<ApiResult<List<KyNhanDto>>> GetAll(ManageKyNhanPagingRequest request);
        Task<PagedResult<KyNhanDto>> GetManageListPaging(ManageKyNhanPagingRequest request);
        Task<ApiResult<int>> CreateAsync(KyNhanRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<KyNhanDto>> GetById(int KyNhanId);
        Task<ApiResult<int>> UpdateAsync(KyNhanRequest request);
    }
    public class KyNhanService : IKyNhanService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public KyNhanService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<List<KyNhanDto>>> GetAll(ManageKyNhanPagingRequest request)
        {
            var query = from p in _context.KyNhans
                        select new { p };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.p.TenNguoiKy.Contains(request.TextSearch) || x.p.VanDon.Code.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<KyNhanDto>>(await query.Select(x => new KyNhanDto()
            {
                Id = x.p.Id,
                BuuCucId = x.p.BuuCucId,
                TenNguoiKy = x.p.TenNguoiKy,
                NgayKyNhan = x.p.NgayKyNhan,
                MaVanDon = x.p.VanDon.Code
            }).AsNoTracking().ToListAsync());
        }
        public async Task<ApiResult<KyNhanDto>> GetById(int id)
        {
            var KyNhan = await _context.KyNhans.FindAsync(id);

            var KyNhanDto = new KyNhanDto()
            {
                Id = KyNhan.Id,
                CreatedDate = KyNhan.NgayKyNhan,
                BuuCucId = KyNhan.BuuCucId,
                MaVanDon = KyNhan.VanDon.Code
            };

            return new ApiSuccessResult<KyNhanDto>(KyNhanDto);
        }
        public async Task<ApiResult<int>> CreateAsync(KyNhanRequest request)
        {
            try
            {
                var KyNhan = new KyNhan()
                {
                    BuuCucId = request.BuuCucId,
                    VanDonId = request.VanDonId,
                    TenNguoiKy = request.TenNguoiKy,
                    NgayKyNhan = DateTime.Now
                };

                _context.KyNhans.Add(KyNhan);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(KyNhan.Id);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
        public async Task<ApiResult<int>> UpdateAsync(KyNhanRequest request)
        {
            try
            {
                var KyNhan = await _context.KyNhans.FindAsync(request.Id);

                if (KyNhan == null) throw new ApplicationException($"Không tìm thấy Giới tính có id: {request.Id}");

                KyNhan.BuuCucId = request.BuuCucId;
                KyNhan.VanDonId = request.VanDonId;
                KyNhan.TenNguoiKy = request.TenNguoiKy;
                KyNhan.NgayKyNhan = DateTime.Now;
                _context.KyNhans.Update(KyNhan);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
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
                var KyNhans = await _context.KyNhans.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (KyNhans == null) throw new GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

                _context.KyNhans.RemoveRange(KyNhans);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<KyNhanDto>> GetManageListPaging(ManageKyNhanPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.KyNhans
                        select new { p };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.p.TenNguoiKy.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new KyNhanDto()
            {
                Id = x.p.Id,
                BuuCucId = x.p.BuuCucId,
                CreatedDate = x.p.NgayKyNhan,
                MaVanDon = x.p.VanDon.Code
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<KyNhanDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }
    }
}
