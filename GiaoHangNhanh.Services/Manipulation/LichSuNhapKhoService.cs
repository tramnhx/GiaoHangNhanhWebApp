using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuNhapKhos;
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
    public interface ILichSuNhapKhoService
    {
        Task<PagedResult<LichSuNhapKhoDto>> GetManageListPaging(ManageLichSuNhapKhoPagingRequest request);
        Task<ApiResult<int>> CreateAsync(LichSuNhapKhoRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<int>> UpdateAsync(LichSuNhapKhoRequest request);
    }
    public class LichSuNhapKhoService : ILichSuNhapKhoService
    {
        private readonly GiaoHangNhanhDbContext _context;

        public LichSuNhapKhoService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<int>> CreateAsync(LichSuNhapKhoRequest request)
        {
            try
            {
                var lichSuNhapKho = new LichSuNhapKho()
                {
                    BuuCucGuiHangId= request.BuuCucGuiHangId,
                    VanDonId = request.VanDonId,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = request.CreatedUserId,
                    ModifiedUserId= request.ModifiedUserId,
                };
                _context.LichSuNhapKhos.Add(lichSuNhapKho);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(lichSuNhapKho.Id);
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
                var lichSuNhapKhos = await _context.LichSuNhapKhos.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (lichSuNhapKhos == null) throw new GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");

                foreach (var item in lichSuNhapKhos)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.LichSuNhapKhos.UpdateRange(lichSuNhapKhos);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }     
        public async Task<PagedResult<LichSuNhapKhoDto>> GetManageListPaging(ManageLichSuNhapKhoPagingRequest request)
        {
            //1. Select join 
            var query = from nk in _context.LichSuNhapKhos
                        join v in _context.VanDons on nk.VanDonId equals v.Id
                        join b in _context.BuuCucs on nk.BuuCucGuiHangId equals b.Id
                        join au in _context.AppUsers on v.NhanVienLayHangId equals au.Id
                        where nk.IsDeleted == false
                        select new { nk,v,b,au };
            
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.b.Name.Contains(request.TextSearch));
            if (request.FilterByBuuCucGuiHangId != null)
            {
                query = query.Where(x => x.nk.Id == request.FilterByBuuCucGuiHangId.Value);
            }
            if (request.FilterByUserId != null)
            {
                query = query.Where(x => x.nk.Id == request.FilterByUserId.Value);
            }
            var a = await query.CountAsync();
            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new LichSuNhapKhoDto()
            {
                Id = x.nk.Id,
                CreatedDate = x.nk.CreatedDate,
                VanDon = new VanDonDto()
                {
                    Id = x.v.Id,
                    Code = x.v.Code,
                    Name = x.v.Name,
                    CreatedDate = x.v.CreatedDate,
                    ModifiedDate = x.v.ModifiedDate,
                    Description = x.v.Description,
                    CreatedUserId = x.v.CreatedUserId,
                    ModifiedUserId = x.v.ModifiedUserId,
                    
                    SortOrder = x.v.SortOrder,
                    IsDeleted = x.v.IsDeleted,
                    CongTyGuiHang = new CongTyGuiHangDto()
                    {
                        Id = x.v.CongTyGuiHang.Id,
                        Code = x.v.CongTyGuiHang.Code,
                        Name = x.v.CongTyGuiHang.Name
                    },
                    User = new UserDto()
                    {
                        Id = x.au.Id.ToString(),
                        UserName = x.au.UserName,
                        FullName = $"{x.au.LastName} {x.au.FirstName}"
                    },
                    DichVu = new DichVuDto()
                    {
                        Id = x.v.DichVu.Id,
                        Code = x.v.DichVu.Code,
                        Name = x.v.DichVu.Name,
                    }
                },
                BuuCuc = new BuuCucDto()
                {
                    Id = x.b.Id,
                    Code = x.b.Code,
                    Name = x.b.Name,
                },
                //User = x.nk.AppUser,
            }).AsNoTracking().ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<LichSuNhapKhoDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(LichSuNhapKhoRequest request)
        {
            try
            {
                var LichSuNhapKho = await _context.LichSuNhapKhos.FindAsync(request.Id);

                if (LichSuNhapKho == null) throw new GiaoHangNhanhException($"Không tìm thấy lịch sử nhập kho có id: {request.Id}");

                //LichSuNhapKho.SortOrder = request.SortOrder;
                LichSuNhapKho.BuuCucGuiHangId = request.BuuCucGuiHangId;
                LichSuNhapKho.VanDonId = request.VanDonId;
                LichSuNhapKho.ModifiedDate = DateTime.Now;
                LichSuNhapKho.ModifiedUserId = request.ModifiedUserId;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
    }
}
