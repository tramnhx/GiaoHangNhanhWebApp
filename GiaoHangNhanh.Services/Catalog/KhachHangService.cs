using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhachHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Catalog
{
    public interface IKhachHangService
    {
        Task<ApiResult<List<KhachHangDto>>> GetAll(ManageKhachHangPagingRequest request);
        Task<PagedResult<KhachHangDto>> GetManageListPaging(ManageKhachHangPagingRequest request);
        Task<ApiResult<int>> CreateAsync(KhachHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<KhachHangDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(KhachHangRequest request);
    }
    public class KhachHangService : IKhachHangService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public KhachHangService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> CreateAsync(KhachHangRequest request)
        {
            try
            {
                var khachHang = new KhachHang()
                {
                    Name = request.Name,
                    Code = request.Code,
                    PhoneNumber = request.PhoneNumber,
                    CCCD = request.CCCD,
                    DiaChi = request.DiaChi,
                    Description = request.Description,
                    Email = request.Email,
                    CreatedUserId = request.CreatedUserId
                };

                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(khachHang.Id);
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
                var khachHang = await _context.KhachHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (khachHang == null) throw new ApplicationException($"Can't find Id: {string.Join(";", request.Ids)}");

                foreach (var item in khachHang)
                {
                    item.IsDeleted = true;
                    item.ModifiedDate = DateTime.Now;
                    //item.ModifiedUserId = request.DeleteUserId;
                    _context.KhachHangs.Update(item);
                }

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<List<KhachHangDto>>> GetAll(ManageKhachHangPagingRequest request)
        {
            var query = from p in _context.KhachHangs
                        where p.IsDeleted == false
                        select new { p };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.p.Name.Contains(request.TextSearch) || x.p.PhoneNumber.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<KhachHangDto>>(await query.Select(x => new KhachHangDto()
            {
                Id = x.p.Id,
                Code = x.p.Code,
                Name = x.p.Name,
                PhoneNumber = x.p.PhoneNumber,
                CCCD = x.p.CCCD,
                Description = x.p.Description,
                DiaChi = x.p.DiaChi,
                Email = x.p.Email,
                CreatedDate = x.p.CreatedDate,
                ModifiedDate = x.p.ModifiedDate,
                CreatedUserId = x.p.CreatedUserId,
                ModifiedUserId = x.p.ModifiedUserId,
                IsDeleted = x.p.IsDeleted,
            }).AsNoTracking().ToListAsync());
        }

        public async Task<ApiResult<KhachHangDto>> GetById(int id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);

            var khachHangDto = new KhachHangDto()
            {
                Id = khachHang.Id,
                Code = khachHang.Code,
                Name = khachHang.Name,
                PhoneNumber = khachHang.PhoneNumber,
                CCCD = khachHang.CCCD,
                Description = khachHang.Description,
                DiaChi = khachHang.DiaChi,
                Email = khachHang.Email,
                CreatedDate = khachHang.CreatedDate,
                ModifiedDate = khachHang.ModifiedDate,
                CreatedUserId = khachHang.CreatedUserId,
                ModifiedUserId = khachHang.ModifiedUserId,
            };

            return new ApiSuccessResult<KhachHangDto>(khachHangDto);
        }

        public async Task<PagedResult<KhachHangDto>> GetManageListPaging(ManageKhachHangPagingRequest request)
        {
            //1. Select join
            var query = from p in _context.KhachHangs
                        where p.IsDeleted == false
                        select new { p };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.p.Name.Contains(request.TextSearch) || x.p.PhoneNumber.Contains(request.TextSearch));

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new KhachHangDto()
            {
                Id = x.p.Id,
                Code = x.p.Code,
                Name = x.p.Name,
                PhoneNumber = x.p.PhoneNumber,
                CCCD = x.p.CCCD,
                Description = x.p.Description,
                DiaChi = x.p.DiaChi,
                Email = x.p.Email,
                CreatedDate = x.p.CreatedDate,
                ModifiedDate = x.p.ModifiedDate,
                CreatedUserId = x.p.CreatedUserId,
                ModifiedUserId = x.p.ModifiedUserId
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<KhachHangDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(KhachHangRequest request)
        {
            try
            {
                var khachHang = await _context.KhachHangs.FindAsync(request.Id);

                if (khachHang == null) throw new ApplicationException($"Không tìm thấy Giới tính có id: {request.Id}");

                khachHang.Code = request.Code;
                khachHang.Name = request.Name;
                khachHang.CCCD = request.CCCD;
                khachHang.DiaChi = request.DiaChi;
                khachHang.Description = request.Description;
                khachHang.PhoneNumber = request.PhoneNumber;
                khachHang.Email = request.Email;
                khachHang.ModifiedDate = DateTime.Now;
                khachHang.ModifiedUserId = request.ModifiedUserId;
                _context.KhachHangs.Update(khachHang);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
