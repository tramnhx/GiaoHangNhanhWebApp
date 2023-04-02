using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DuyetChuyenHoans;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface IDuyetChuyenHoanService
    {
        Task<PagedResult<DuyetChuyenHoanDto>> GetManageListPaging(ManageDuyetChuyenHoanPagingRequest request);
        Task<ApiResult<List<DuyetChuyenHoanDto>>> GetAll(ManageDuyetChuyenHoanPagingRequest request);
        Task<ApiResult<int>> CreateAsync(DuyetChuyenHoanRequest request);
        Task<ApiResult<int>> UpdateAsync(DuyetChuyenHoanRequest request);

    }
    public class DuyetChuyenHoanService : IDuyetChuyenHoanService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public DuyetChuyenHoanService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> CreateAsync(DuyetChuyenHoanRequest request)
        {
            try
            {
                var duyetchuyenHoan = new DuyetChuyenHoan()
                {

                    VanDonId = request.VanDonId,
                    DangKyChuyenHoanId = request.DangKyChuyenHoanId,
                    NguoiKyNhanChuyenHoan = request.NguoiKyNhanChuyenHoan,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId = request.ModifiedUserId,
                    CreatedDate =DateTime.Now

                };
                _context.DuyetChuyenHoans.Add(duyetchuyenHoan);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(duyetchuyenHoan.Id);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<List<DuyetChuyenHoanDto>>> GetAll(ManageDuyetChuyenHoanPagingRequest request)
        {
            var query = from dch in _context.DuyetChuyenHoans
                        .Where(x => x.IsDeleted == false)
                        join v in _context.VanDons on dch.VanDonId equals v.Id
                        join d in _context.DangKyChuyenHoans on dch.DangKyChuyenHoanId equals d.Id

                        select new { dch, v, d };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.dch.Name.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<DuyetChuyenHoanDto>>(await query.Select(x => new DuyetChuyenHoanDto()
            {
                Id = x.dch.Id,
                Name = x.dch.Name,
                Code = x.dch.Code,
                CreatedDate = x.dch.CreatedDate,
                ModifiedDate = x.dch.ModifiedDate,
                CreatedUserId = x.dch.CreatedUserId,
                ModifiedUserId = x.dch.ModifiedUserId,
                Description = x.dch.Description,
                SortOrder = x.dch.SortOrder,
                IsDeleted = x.dch.IsDeleted,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Name = x.v.Name,

                },
                DangKyChuyenHoan = new DangKyChuyenHoanDto()
                {
                    Id = x.d.Id,
                    NguyenNhan = x.d.NguyenNhan,
                },
            }).AsNoTracking().ToListAsync());
        }

        public async Task<PagedResult<DuyetChuyenHoanDto>> GetManageListPaging(ManageDuyetChuyenHoanPagingRequest request)
        {
            //1. Select join
            var query = from dch in _context.DuyetChuyenHoans
                        .Where(x => x.IsDeleted == false)
                        join dkch in _context.DangKyChuyenHoans on dch.DangKyChuyenHoanId equals dkch.Id
                        join vd in _context.VanDons on dch.VanDonId equals vd.Id
                        join bc in _context.BuuCucs on vd.BuuCucHangDenId equals bc.Id
                        join dv in _context.DichVus on vd.DichVuId equals dv.Id
                        join bcdh in _context.BuuCucs on dch.BuuCucDuyetHangId equals bcdh.Id into dch_bc from bcdh in dch_bc.DefaultIfEmpty()
                        select new { dch, dkch, vd, bc, dv, bcdh };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.vd.Code.Contains(request.TextSearch));

            if (request.FilterByDangKyChuyenHoanId != null)
            {
                query = query.Where(x => x.dch.Id == request.FilterByDangKyChuyenHoanId.Value);
            }
            if (request.FilterByVanDonId != null)
            {
                query = query.Where(x => x.vd.Id == request.FilterByVanDonId.Value);
            }
            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }

            var data = await query.Select(x => new DuyetChuyenHoanDto()
            {
                Id = x.dch.Id,
                MaVanDon = x.vd.Code,
                BuuCucGuiHang = x.bc.Code,
                PhanLoaiChuyenPhat = x.dv.Name,
                TenKhachHang = x.vd.HoTenNguoiNhan,
                RutVeDichDen = x.vd.DiaChiNguoiGui,
                NguyenNhanChuyenHoan = x.dkch.NguyenNhan,
                NgayDuyetChuyenHoan = x.dch.NgayDuyetChuyenHoan,
                BuuCucDuyetChuyenHoan = x.bcdh.Code,
                IsDaDuyet = x.dch.IsDaDuyet == true ? "Đã duyệt" : String.Empty,
                
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<DuyetChuyenHoanDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(DuyetChuyenHoanRequest request)
        {
            try
            {
                var duyetChuyenHoan = await _context.DuyetChuyenHoans.FindAsync(request.Id);

                if (duyetChuyenHoan.IsDeleted == true || duyetChuyenHoan == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                duyetChuyenHoan.NguoiKyNhanChuyenHoan = request.NguoiKyNhanChuyenHoan;
                duyetChuyenHoan.BuuCucDuyetHangId = request.BuuCucDuyetChuyenHoanId;
                duyetChuyenHoan.NgayDuyetChuyenHoan = DateTime.Now;
                duyetChuyenHoan.IsDaDuyet = true;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
