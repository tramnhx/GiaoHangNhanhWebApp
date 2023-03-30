using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
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
            var query = from k in _context.KyNhans
                        join vd in _context.VanDons on k.VanDonId equals vd.Id
                        join bc in _context.BuuCucs on k.BuuCucId equals bc.Id
                        select new { k, vd, bc };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.k.TenNguoiKy.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<KyNhanDto>>(await query.Select(x => new KyNhanDto()
            {
                Id = x.k.Id,
                TenNguoiKy = x.k.TenNguoiKy,
                DauKyThay = x.k.DauKyThay,
                NgayKyNhan = x.k.NgayKyNhan.ToString("dd/MM/yyyy HH:mm:ss"),
                Description = x.k.Description,
                BuuCuc = new BuuCucDto()
                {
                    Id = x.bc.Id,
                    Code = x.bc.Code
                },
                VanDon = new VanDonDto()
                {
                    Id = x.vd.Id,
                    Code = x.vd.Code
                }
            }).AsNoTracking().ToListAsync());
        }
        public async Task<ApiResult<KyNhanDto>> GetById(int id)
        {
            var KyNhan = await _context.KyNhans.FindAsync(id);
            if (KyNhan.IsDeleted == true || KyNhan == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {id}");

            var KyNhanDto = new KyNhanDto()
            {
                Id = KyNhan.Id,
                CreatedDate = KyNhan.NgayKyNhan,
                TenNguoiKy = KyNhan.TenNguoiKy,
                DauKyThay = KyNhan.DauKyThay,
                VanDonId = KyNhan.VanDonId,
                Description = KyNhan.Description,
                BuuCuc = new BuuCucDto()
                {
                    Id = KyNhan.BuuCuc.Id,
                    Name = KyNhan.BuuCuc.Code
                }
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
                    NhanVienPhat = request.NhanVienPhat,
                    TenNguoiKy = request.TenNguoiKy,
                    DauKyThay = request.DauKyThay,
                    Description = request.Description,
                    CreatedUserId = request.CreatedUserId,
                    NgayKyNhan = DateTime.Now,
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

                if (KyNhan == null) throw new GiaoHangNhanh.Utilities.Exceptions.GiaoHangNhanhException($"Không tìm thấy nhân viên có id: {request.Id}");

                KyNhan.BuuCucId = request.BuuCucId;
                KyNhan.NhanVienPhat = request.NhanVienPhat;
                KyNhan.VanDonId = request.VanDonId;
                KyNhan.TenNguoiKy = request.TenNguoiKy;
                KyNhan.DauKyThay = request.DauKyThay;
                KyNhan.Description = request.Description;
                KyNhan.NhanVienPhat = request.NhanVienPhat;
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
                if (KyNhans == null) throw new GiaoHangNhanh.Utilities.Exceptions.GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

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
            var query = from k in _context.KyNhans
                         .Where(x => x.IsDeleted == false)
                        join vd in _context.VanDons on k.VanDonId equals vd.Id
                        join bc in _context.BuuCucs on k.BuuCucId equals bc.Id
                        join nv in _context.NhanViens on vd.NhanVienId equals nv.Id
                        select new { k, vd, bc, nv };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.k.TenNguoiKy.Contains(request.TextSearch));

            if (request.FilterByVanDon != null)
            {
                query = query.Where(x => x.k.Id == request.FilterByVanDon.Value);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }

            var data = await query.Select(x => new KyNhanDto()
            {
                Id = x.k.Id,
                NgayKyNhan = x.k.NgayKyNhan.ToString("dd/MM/yyyy HH:mm:ss"),
                DauKyThay = x.k.DauKyThay,
                TenNguoiKy = x.k.TenNguoiKy,
                Description = x.k.Description,
                BuuCuc = new BuuCucDto()
                {
                    Id = x.bc.Id,
                    Code = x.bc.Code
                },
                VanDon = new VanDonDto()
                {
                    Id = x.vd.Id,
                    Code = x.vd.Code,
                    NhanVien = new NhanVienDto()
                    {
                        Id = x.nv.Id,
                        FullName = $"{x.nv.LastName} {x.nv.FirstName}"
                    }
                }
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
