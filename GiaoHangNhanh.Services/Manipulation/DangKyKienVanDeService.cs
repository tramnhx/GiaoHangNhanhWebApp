using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhanLoaiHangBatThuongs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyKienVanDes;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface IDangKyKienVanDeService
    {
        Task<PagedResult<DangKyKienVanDeDto>> GetManageListPaging(ManageDangKyKienVanDePagingRequest request);
        Task<ApiResult<int>> CreateAsync(DangKyKienVanDeRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<DangKyKienVanDeDto>> GetById(int? id);
        Task<ApiResult<int>> UpdateAsync(DangKyKienVanDeRequest request);
    }
    public class DangKyKienVanDeService : IDangKyKienVanDeService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public DangKyKienVanDeService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> CreateAsync(DangKyKienVanDeRequest request)
           {
            try
            {
                var dangKyKienVanDe = new DangKyKienVanDe()
                {
                    VanDonId = request.VanDonId,
                    PhanLoaiHangBatThuongId = request.PhanLoaiHangBatThuongId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(request.CreatedUserId),
                };
                _context.DangKyKienVanDes.Add(dangKyKienVanDe);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(dangKyKienVanDe.Id);
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
                var dangKyKienVanDes = await _context.DangKyKienVanDes.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (dangKyKienVanDes == null) throw new GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

                foreach (var item in dangKyKienVanDes)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.DangKyKienVanDes.UpdateRange(dangKyKienVanDes);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<DangKyKienVanDeDto>> GetById(int? id)
        {
            var dangKyKienVanDe = await (from dkkvd in _context.DangKyKienVanDes
                                .Where(x => x.Id == id && x.IsDeleted == false)
                                        join p in _context.PhanLoaiHangBatThuongs on dkkvd.PhanLoaiHangBatThuongId equals p.Id
                                        join v in _context.VanDons on dkkvd.VanDonId equals v.Id
                                        select new DangKyKienVanDeDto()
                                        {
                                            Id = dkkvd.Id,
                                            Code = dkkvd.Code,
                                            CreatedDate = dkkvd.CreatedDate,
                                            ModifiedDate = dkkvd.ModifiedDate,
                                            CreatedUserId = dkkvd.CreatedUserId,
                                            ModifiedUserId = dkkvd.ModifiedUserId,
                                            SortOrder = dkkvd.SortOrder,
                                            IsDeleted = dkkvd.IsDeleted,
                                            VanDon = new VanDonDto()
                                            {
                                                Id = v.Id,
                                                Code = v.Code,
                                                Name = v.Name,

                                                BuuCuc = new BuuCucDto()
                                                {
                                                    Id = v.Id,
                                                    Code = v.Code,
                                                    Name = v.Name,
                                                },
                                                CongTyGuiHang = new CongTyGuiHangDto()
                                                {
                                                    Id = v.Id,
                                                    Code = v.Code,
                                                    Name = v.Name,
                                                },
                                            },
                                            PhanLoaiHangBatThuong = new PhanLoaiHangBatThuongDto()
                                            {
                                                Id = p.Id,
                                                Code = p.Code,
                                                Name = p.Name
                                            },
                                        }).AsNoTracking().FirstOrDefaultAsync();

            return new ApiSuccessResult<DangKyKienVanDeDto>(dangKyKienVanDe);
        }

        public async Task<PagedResult<DangKyKienVanDeDto>> GetManageListPaging(ManageDangKyKienVanDePagingRequest request)
        {
            //1. Select join
            var query = from gh in _context.DangKyKienVanDes
                        .Where(x => x.IsDeleted == false)
                        join v in _context.VanDons on gh.VanDonId equals v.Id
                        join p in _context.PhanLoaiHangBatThuongs on gh.PhanLoaiHangBatThuongId equals p.Id
                        select new { gh, v, p };
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.gh.Name.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new DangKyKienVanDeDto()
            {
                Id = x.gh.Id,
                Code = x.gh.Code,
                SortOrder = x.gh.SortOrder,
                IsDeleted = x.gh.IsDeleted,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    Name = x.v.Name,
                    HoTenNguoiGui = x.v.HoTenNguoiGui,
                    HoTenNguoiNhan = x.v.HoTenNguoiNhan,
                    DienThoaiNguoiGui =x.v.DienThoaiNguoiGui,
                    DienThoaiNguoiNhan =x.v.DienThoaiNguoiNhan,
                    DiaChiNguoiGui = x.v.DiaChiNguoiGui,
                    DiaChiNguoiNhan = x.v.DiaChiNguoiNhan,
                    NgayGuiHang = x.v.NgayGuiHang,
                    BuuCuc = new BuuCucDto()
                    {
                        Id = x.v.Id,
                        Code = x.v.Code,
                        Name = x.v.Name,
                    },
                    CongTyGuiHang = new CongTyGuiHangDto()
                    {
                        Id = x.v.Id,
                        Code = x.v.Code,
                        Name = x.v.Name,
                    },
                },
                PhanLoaiHangBatThuong = new PhanLoaiHangBatThuongDto()
                {
                    Id = x.p.Id,
                    Code = x.p.Code,
                    Name = x.p.Name,
                },
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<DangKyKienVanDeDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(DangKyKienVanDeRequest request)
        {
            try
            {
                var DangKyKienVanDe = await _context.DangKyKienVanDes.FindAsync(request.Id);

                if (DangKyKienVanDe == null) throw new ApplicationException($"Không tìm thấy id: {request.Id}");

                DangKyKienVanDe.VanDonId = request.VanDonId;
                DangKyKienVanDe.PhanLoaiHangBatThuongId = request.PhanLoaiHangBatThuongId;
                DangKyKienVanDe.ModifiedDate = DateTime.Now;

                _context.DangKyKienVanDes.Update(DangKyKienVanDe);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
