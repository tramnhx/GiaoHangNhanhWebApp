using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuPhatHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface ILichSuPhatHangService
    { 
        Task<PagedResult<LichSuPhatHangDto>> GetManageListPaging(ManageLichSuPhatHangPagingRequest request);
        Task<ApiResult<int>> CreateAsync(LichSuPhatHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<LichSuPhatHangDto>> GetById(int? id);
        Task<ApiResult<int>> UpdateAsync(LichSuPhatHangRequest request);
    }
    public class LichSuPhatHangService : ILichSuPhatHangService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public LichSuPhatHangService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> CreateAsync(LichSuPhatHangRequest request)
        {
            try
            {
                 var lichSuPhatHang = new LichSuPhatHang()
                {
                    NhanVienId = request.NhanVienId,
                    VanDonId = request.VanDonId,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedUserId = Guid.Parse(request.CreatedUserId),
                 };
                _context.LichSuPhatHangs.Add(lichSuPhatHang);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(lichSuPhatHang.Id);
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
                var lichSuPhatHangs = await _context.LichSuPhatHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (lichSuPhatHangs == null) throw new GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

                foreach (var item in lichSuPhatHangs)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuPhatHangs.UpdateRange(lichSuPhatHangs);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<LichSuPhatHangDto>> GetById(int? id)
        {
            var lichSuPhatHang = await (from lsph in _context.LichSuPhatHangs
                                .Where(x => x.Id == id && x.IsDeleted == false)
                                join a in _context.NhanViens on lsph.NhanVienId equals a.Id
                                join v in _context.VanDons on lsph.VanDonId equals v.Id
                                select new LichSuPhatHangDto()
                                {
                                    Id = lsph.Id,
                                    Code = lsph.Code,
                                    CreatedDate = lsph.CreatedDate,
                                    ModifiedDate = lsph.ModifiedDate,
                                    CreatedUserId = lsph.CreatedUserId,
                                    ModifiedUserId = lsph.ModifiedUserId,
                                    SortOrder = lsph.SortOrder,
                                    IsDeleted = lsph.IsDeleted,
                                    VanDon = new VanDonDto()
                                    {
                                        Id = v.Id,
                                        Code = v.Code,
                                        Name = v.Name
                                    },
                                    NhanVien = new NhanVienDto()
                                    {
                                        FullName = $"{a.LastName} {a.FirstName}"
                                    }
                                }).AsNoTracking().FirstOrDefaultAsync();

            return new ApiSuccessResult<LichSuPhatHangDto>(lichSuPhatHang);
        }

        public async Task<PagedResult<LichSuPhatHangDto>> GetManageListPaging(ManageLichSuPhatHangPagingRequest request)
        {
            //1. Select join
            var query = from gh in _context.LichSuPhatHangs
                        .Where(x => x.IsDeleted == false)
                        join v in _context.VanDons on gh.VanDonId equals v.Id
                        join a in _context.NhanViens on gh.NhanVienId equals a.Id
                        select new { gh, v, a};
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.gh.Name.Contains(request.TextSearch));
            if (request.FilterByNhanVienId != null)
            {
                query = query.Where(x => x.gh.Id == request.FilterByNhanVienId.Value);
            }
            if (request.FilterByStaffId != null)
            {
                query = query.Where(x => x.gh.Id == request.FilterByStaffId.Value);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new LichSuPhatHangDto()
            {
                Id = x.gh.Id,
                Code = x.gh.Code,
                StrCreatedDay = x.gh.CreatedDate.ToString("dd/MM/yyyy"),
                SortOrder = x.gh.SortOrder,
                IsDeleted = x.gh.IsDeleted,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    Name = x.v.Name,
                },
                NhanVien = new NhanVienDto()
                {
                    Id = x.a.Id,
                    Code = x.a.Code,
                    FullName = $"{x.a.LastName} {x.a.FirstName}"
                }
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<LichSuPhatHangDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuPhatHangRequest request)
        {
            try
            {
                var LichSuPhatHang = await _context.LichSuPhatHangs.FindAsync(request.Id);

                if (LichSuPhatHang == null) throw new ApplicationException($"Không tìm thấy id: {request.Id}");

                LichSuPhatHang.VanDonId = request.VanDonId;
                LichSuPhatHang.ModifiedDate = DateTime.Now;
                LichSuPhatHang.NhanVienId = request.NhanVienId;

                _context.LichSuPhatHangs.Update(LichSuPhatHang);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
