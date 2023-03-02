using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.PhanLoaiVanDons;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface IPhanLoaiVanDonService
    {
        Task<ApiResult<List<PhanLoaiVanDonDto>>> GetAll(ManagePhanLoaiVanDonPagingRequest request);
        Task<PagedResult<PhanLoaiVanDonDto>> GetManageListPaging(ManagePhanLoaiVanDonPagingRequest request);
        Task<ApiResult<int>> CreateAsync(PhanLoaiVanDonRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<PhanLoaiVanDonDto>> GetById(int idHangBatThuong);
        Task<ApiResult<int>> UpdateAsync(PhanLoaiVanDonRequest request);
    }
    public class PhanLoaiVanDonService : IPhanLoaiVanDonService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public PhanLoaiVanDonService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> CreateAsync(PhanLoaiVanDonRequest request)
        {
            try
            {
                var hangBatThuong = new PhanLoaiVanDon()
                {
                    Name = request.Ten,
                    Description = request.GhiChu,
                    SortOrder = request.SortOrder,
                    MaVanDon = request.MaVanDon,
                    IdDMPhanLoaiHangBT = request.IdDMPhanLoaiHangBT,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = request.CreatedUserId,
                };
                _context.PhanLoaiVanDons.Add(hangBatThuong);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(hangBatThuong.Id);
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
                var hangBatThuong = await _context.PhanLoaiVanDons.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (hangBatThuong == null) throw new GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

                foreach (var item in hangBatThuong)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.PhanLoaiVanDons.UpdateRange(hangBatThuong);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<List<PhanLoaiVanDonDto>>> GetAll(ManagePhanLoaiVanDonPagingRequest request)
        {
            var query = from p in _context.PhanLoaiVanDons
                        .Where(x => x.IsDeleted == false)
                        select new { p };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.p.MaVanDon.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<PhanLoaiVanDonDto>>(await query.Select(x => new PhanLoaiVanDonDto()
            {
                Id = x.p.Id,
                MaVanDon = x.p.MaVanDon,
                IdDMPhanLoaiHangBT = x.p.IdDMPhanLoaiHangBT,
                CreatedDate = x.p.CreatedDate

            }).AsNoTracking().ToListAsync());
        }

        public async Task<ApiResult<PhanLoaiVanDonDto>> GetById(int idHangBatThuong)
        {
            var hangBatThuong = await _context.PhanLoaiVanDons.FindAsync(idHangBatThuong);

            var hangBatThuongDto = new PhanLoaiVanDonDto()
            {
                Id = hangBatThuong.Id,
                MaVanDon = hangBatThuong.MaVanDon,
                IdDMPhanLoaiHangBT = hangBatThuong.IdDMPhanLoaiHangBT,
                CreatedDate = hangBatThuong.CreatedDate,
                IsDeleted = hangBatThuong.IsDeleted,
                ModifiedDate = hangBatThuong.ModifiedDate,
                CreatedUserId = hangBatThuong.CreatedUserId,
                ModifiedUserId = hangBatThuong.ModifiedUserId,
            };

            return new ApiSuccessResult<PhanLoaiVanDonDto>(hangBatThuongDto);
        }

        public async Task<PagedResult<PhanLoaiVanDonDto>> GetManageListPaging(ManagePhanLoaiVanDonPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.PhanLoaiVanDons
                        .Where(x => x.IsDeleted == false)
                        select new { p };

            //2. filter
            //if (!string.IsNullOrEmpty(request.TextSearch))
            //    query = query.Where(x => x.p.DanhMucPhanLoaiHangBT.Ten.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new PhanLoaiVanDonDto()
            {
                Id = x.p.Id,
                MaVanDon = x.p.MaVanDon,
                IdDMPhanLoaiHangBT = x.p.IdDMPhanLoaiHangBT,
                CreatedDate = x.p.CreatedDate,
            }).AsNoTracking().Where(x => x.IsDeleted == false).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<PhanLoaiVanDonDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(PhanLoaiVanDonRequest request)
        {
            try
            {
                var hangBatThuong = await _context.PhanLoaiVanDons.FindAsync(request.Id);

                if (hangBatThuong == null) throw new GiaoHangNhanhException($"Không tìm thấy hàng bất thường có id: {request.Id}");
                hangBatThuong.Name = request.Ten;
                hangBatThuong.Description = request.GhiChu;
                hangBatThuong.SortOrder = request.SortOrder;
                hangBatThuong.IdDMPhanLoaiHangBT = request.IdDMPhanLoaiHangBT;
                hangBatThuong.MaVanDon = request.MaVanDon;
                hangBatThuong.ModifiedDate = DateTime.Now;
                hangBatThuong.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
