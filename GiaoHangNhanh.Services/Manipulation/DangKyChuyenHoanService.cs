using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans;
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
    public interface IDangKyChuyenHoanService
    {
        Task<PagedResult<DangKyChuyenHoanDto>> GetManageListPaging(ManageDangKyChuyenHoanPagingRequest request);
        Task<ApiResult<int>> CreateAsync(DangKyChuyenHoanRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<int>> UpdateAsync(DangKyChuyenHoanRequest request);
    }
    public class DangKyChuyenHoanService : IDangKyChuyenHoanService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public DangKyChuyenHoanService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> CreateAsync(DangKyChuyenHoanRequest request)
        {
            try
            {
                var chuyenHoan = new DangKyChuyenHoan()
                {
              
                    VanDonId = request.VanDonId,
                    MieuTaNguyenNhan = request.MieuTaNguyenNhan,
                    NguyenNhan = request.NguyenNhan,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    
                };
                _context.DangKyChuyenHoans.Add(chuyenHoan);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(chuyenHoan.Id);
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
                var chuyenHoans = await _context.DangKyChuyenHoans.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (chuyenHoans == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in chuyenHoans)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.DangKyChuyenHoans.UpdateRange(chuyenHoans);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<PagedResult<DangKyChuyenHoanDto>> GetManageListPaging(ManageDangKyChuyenHoanPagingRequest request)
        {
            //1. Select join
            var query = from dk in _context.DangKyChuyenHoans 
                        .Where(x => x.IsDeleted == false)
                        join v in _context.VanDons on dk.VanDonId equals v.Id
                        select new { dk, v };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.dk.NguyenNhan.Contains(request.TextSearch));


            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new DangKyChuyenHoanDto()
            {
                Id = x.dk.Id,
                NguyenNhan = x.dk.NguyenNhan,
                MieuTaNguyenNhan = x.dk.MieuTaNguyenNhan,              
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    TrongLuong = x.v.TrongLuong,
                    DiaChiNguoiGui = x.v.DiaChiNguoiGui
                }

            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<DangKyChuyenHoanDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(DangKyChuyenHoanRequest request)
        {
            try
            {
                var dangKy = await _context.DangKyChuyenHoans.FindAsync(request.Id);

                if (dangKy.IsDeleted == true || dangKy == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                dangKy.MieuTaNguyenNhan = request.MieuTaNguyenNhan;
                dangKy.NguyenNhan = request.NguyenNhan;
                dangKy.ModifiedDate = DateTime.Now;
                dangKy.ModifiedUserId = request.ModifiedUserId;
                dangKy.VanDonId = request.VanDonId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
