using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs;
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
    public interface ILichSuBaoHangService
    {
        Task<PagedResult<LichSuBaoHangDto>> GetManageBaoHangListPaging(ManageLichSuBaoHangPagingRequest request);
        Task<PagedResult<LichSuBaoHangDto>> GetManageVanDonInBaoListPaging(ManageLichSuBaoHangPagingRequest request);
        Task<ApiResult<int>> DongBaoHangAsync(LichSuBaoHangRequest request);
        Task<ApiResult<int>> CreateVanDonInBaoHangAsync(VanDonInBaoRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<int>> DeleteBaoHangIds(DeleteRequest request);
        Task<ApiResult<int>> LayHangRaBao(DeleteRequest request);
        Task<ApiResult<LichSuBaoHangDto>> GetById(int lichSuBaoHangId);
        Task<ApiResult<int>> ThaoBaoHangAsync(LichSuBaoHangRequest request);
    }
    public class LichSuBaoHangService : ILichSuBaoHangService
    {
        private readonly GiaoHangNhanhDbContext _context;
        public LichSuBaoHangService(GiaoHangNhanhDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<int>> DongBaoHangAsync(LichSuBaoHangRequest request)
        {
            try
            {
                if (!await ExistByCode(null, request.Code))
                {
                    var baoHang = new BaoHang()
                    {
                        Code = request.Code,
                        IsDongBao = true,
                        CreatedUserId = request.CreatedUserId,
                        ModifiedUserId = request.CreatedUserId,
                        CreatedDate = DateTime.Now,
                    };

                    _context.BaoHangs.Add(baoHang);
                    await _context.SaveChangesAsync();
                    var a = baoHang.Id;
                    return new ApiSuccessResult<int>(baoHang.Id);
                }
                else
                {
                    return new ApiErrorResult<int>("Đã tồn tại mã này trong hệ thống.");
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
        public async Task<ApiResult<int>> CreateVanDonInBaoHangAsync(VanDonInBaoRequest request)
        {
            try
            {
                    int lichSuBaoHangId = 1;
                    var baoHang = request.BaoHangId;
                    bool ktdh = false;
                    foreach (var item in request.VanDonIds)
                    {
                        ktdh = false;
                        var lichSuBaoHang = new LichSuBaoHang()
                        {
                            VanDonId = item,
                            BaoHangId = baoHang,
                            IsTrongBao = true,
                            CreatedDate = DateTime.Now,
                            CreatedUserId = request.CreatedUserId,
                        };


                        ktdh = await ExistByCodeVanDon(baoHang, item);
                        if (!ktdh)
                        {
                        _context.LichSuBaoHangs.Add(lichSuBaoHang);
                        }
                        
                    }
                    await _context.SaveChangesAsync();

                    return new ApiSuccessResult<int>(lichSuBaoHangId);

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
                var baoHang = await _context.LichSuBaoHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (baoHang == null) throw new GiaoHangNhanh.Utilities.Exceptions.GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");
                foreach (var item in baoHang)
                {
                    if (item.IsTrongBao == true)
                    {
                        _context.LichSuBaoHangs.Remove(item);
                    }
                    else
                    {
                        throw new GiaoHangNhanhException($"Không thể xóa đơn hàng đã tháo");
                    }    
                }

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
        public async Task<ApiResult<int>> DeleteBaoHangIds(DeleteRequest request)
        {
            try
            {
                var baoHang = await _context.BaoHangs.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                
                if (baoHang == null) throw new GiaoHangNhanhException($"Tìm không thấy Id: {string.Join(";", request.Ids)}");
                foreach (var item in baoHang)
                {
                    var vanDon = await _context.LichSuBaoHangs.Where(x => x.BaoHangId == item.Id).FirstOrDefaultAsync();
                    if (vanDon == null)
                    {
                        _context.BaoHangs.Remove(item);
                    }
                    else
                    {
                        throw new GiaoHangNhanhException($"Còn đơn hàng trong bao, không thể xóa!!");
                    }    
                }

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
        public async Task<ApiResult<int>> LayHangRaBao(DeleteRequest request)
        {
            try
            {
                var baoHang = await _context.BaoHangs.FindAsync(request.Id);
                var vandons = await _context.LichSuBaoHangs.Where(m => request.Ids.Contains(m.Id)).ToListAsync();
                if (vandons == null) throw new GiaoHangNhanhException($"Cannot find Id: {String.Join(";", request.Ids)}");
                if (baoHang.IsDongBao == false)
                {
                    foreach (var item in vandons)
                    {
                        item.IsTrongBao = false;
                        _context.Update(item);
                    }
                }
                else
                {
                    throw new GiaoHangNhanhException($"Bao hàng chưa được mở");
                }    
                
                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiResult<int>()
                {
                    IsSuccessed = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<PagedResult<LichSuBaoHangDto>> GetManageVanDonInBaoListPaging(ManageLichSuBaoHangPagingRequest request)
        {
            //1. Select join
            var query = from lsbh in _context.LichSuBaoHangs
                        join bh in _context.BaoHangs on lsbh.BaoHangId equals bh.Id
                        join vd in _context.VanDons on lsbh.VanDonId equals vd.Id
                        join u in _context.AppUsers on lsbh.CreatedUserId equals u.Id
                        select new { lsbh, vd,bh,u };
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.vd.Code.Contains(request.TextSearch));
            if (request.FilterByMaSealBao != null)
            {
                query = query.Where(x => x.bh.Code == request.FilterByMaSealBao);
            }
            //3.Sort
            if (!string.IsNullOrEmpty(request.OrderCol))
            {
                switch (request.OrderCol)
                {
                    case "code":
                        query = (request.OrderDir == "asc") ?
                            query.OrderBy(x => x.vd.Code) :
                            query.OrderByDescending(x => x.vd.Code);

                        break;
                    default: break;
                }
            }


            //4. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new LichSuBaoHangDto()
            {
                Id = x.lsbh.Id,
                AppUser = new UserDto()
                {
                    Id = x.u.Id.ToString(),
                    FullName = $"{x.u.LastName} {x.u.FirstName}",
                },
                VanDon = new VanDonDto()
                {
                    Id = x.vd.Id,
                    Code = x.vd.Code,
                },
                IsTrongBao = x.lsbh.IsTrongBao
                
            }).AsNoTracking().ToListAsync();

            //5. Select and projection
            var pagedResult = new PagedResult<LichSuBaoHangDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<LichSuBaoHangDto>> GetById(int lichSuBaoHangId)
        {
            var lichSuBaoHang = await (from bh in _context.BaoHangs
                                       where bh.Id == lichSuBaoHangId
                                       select new LichSuBaoHangDto()
                                       {
                                           Id = bh.Id,
                                           Code = bh.Code,
                                           IsDongBao = bh.IsDongBao.ToString(),
                                           CreatedDate = bh.CreatedDate.ToString()
                                       }).AsNoTracking().FirstOrDefaultAsync();
            return new ApiSuccessResult<LichSuBaoHangDto>(lichSuBaoHang);
        }

        public async Task<PagedResult<LichSuBaoHangDto>> GetManageBaoHangListPaging(ManageLichSuBaoHangPagingRequest request)
        {
            //1. Select join
            var query = from bh in _context.BaoHangs
                        select new { bh };
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.bh.Code.Contains(request.TextSearch));

            //3.Sort
            if (!string.IsNullOrEmpty(request.OrderCol))
            {
                switch (request.OrderCol)
                {
                    case "code":
                        query = (request.OrderDir == "asc") ?
                            query.OrderBy(x => x.bh.Code) :
                            query.OrderByDescending(x => x.bh.Code);

                        break;
                    default: break;
                }
            }


            //4. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new LichSuBaoHangDto()
            {
                Id = x.bh.Id,
                Code = x.bh.Code,
                IsDongBao = x.bh.IsDongBao ? "Đang đóng" : "Đã mở",
                CreatedDate = x.bh.CreatedDate.ToString("dd/MM/yyyy"),
                MoBaoDate = x.bh.MoBaoDate != null ? x.bh.MoBaoDate.Value.ToString("dd/MM/yyyy") : "Hiện tại chưa mở bao"
            }).AsNoTracking().ToListAsync();

            //5. Select and projection
            var pagedResult = new PagedResult<LichSuBaoHangDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> ThaoBaoHangAsync(LichSuBaoHangRequest request)
        {
            try
            {
                var baoHang = await _context.BaoHangs.FindAsync(request.Id);

                if (baoHang == null) throw new GiaoHangNhanhException($"Không tìm thấy bao hàng có id: {request.Id}");
                baoHang.IsDongBao = false;
                baoHang.MoBaoDate = DateTime.Now;
                baoHang.ModifiedUserId = request.CreatedUserId;
                _context.BaoHangs.Update(baoHang);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
        public async Task<bool> ExistByCode(int? id, string code)
        {
            BaoHang baoHang = null;
            if (id.HasValue)
            {
                baoHang = await _context.BaoHangs.Where(x => x.Code.Trim().ToLower() == code.Trim().ToLower() && x.Id != id.Value).FirstOrDefaultAsync();
            }
            else
            {
                baoHang = await _context.BaoHangs.Where(x => x.Code.Trim().ToLower() == code.Trim().ToLower()).FirstOrDefaultAsync();
            }

            return baoHang != null;
        }
        public async Task<bool> ExistByCodeVanDon(int? baohangId, int vandonId)
        {
            LichSuBaoHang lichSuBaoHang = null;
            lichSuBaoHang = await _context.LichSuBaoHangs.Where(x => x.BaoHangId == baohangId && x.VanDonId == vandonId).FirstOrDefaultAsync();

            return lichSuBaoHang != null;
        }

    }
}
