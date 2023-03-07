using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Genders;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens;
using GiaoHangNhanh.Utilities.Constants;
using GiaoHangNhanh.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.System
{
    public interface INhanVienService
    {
        Task<ApiResult<List<NhanVienDto>>> GetAll(ManageNhanVienPagingRequest request);
        Task<PagedResult<NhanVienDto>> GetManageListPaging(ManageNhanVienPagingRequest request);
        Task<ApiResult<int>> CreateAsync(NhanVienRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<NhanVienDto>> GetById(int id);
        Task<ApiResult<int>> UpdateAsync(NhanVienRequest request);
    }
    public class NhanVienService : INhanVienService
    {
        private readonly GiaoHangNhanhDbContext _context;
        private readonly IConfiguration _configuration;
        public NhanVienService(GiaoHangNhanhDbContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ApiResult<int>> CreateAsync(NhanVienRequest request)
        {
            try
            {
                string code = null;

                DateTime ngayValue;
                if (!await ExistByCode(null, request.Code))
                {
                    if (request.Code.Trim() != null && request.Code != "")
                    {
                        code = request.Code;
                    }
                    else
                    {
                        code = $"NV{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Second}";
                    }
                    var nhanVien = new NhanVien()
                    {

                        Code = code,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        CCCD = request.CCCD,
                        DiaChi = request.DiaChi,
                        BuuCucLamViecId = request.BuuCucLamViecId,
                        SoDienThoai = request.SoDienThoai,
                        Email = request.Email,
                        NgaySinh = (DateTime.TryParseExact(request.NgaySinh, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>()),
                        NgayLamViec = (DateTime.TryParseExact(request.NgayLamViec, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>()),
                        NgayNghiViec = (DateTime.TryParseExact(request.NgayNghiViec, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>()),
                        NoiSinh = request.NoiSinh,
                        GenderId = request.GenderId,
                        IsActive = request.IsActive,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        CreatedUserId = request.CreatedUserId,
                        ModifiedUserId = request.ModifiedUserId,
                    };

                    _context.NhanViens.Add(nhanVien);
                    await _context.SaveChangesAsync();
                    return new ApiSuccessResult<int>(nhanVien.Id);
                }
                else
                {
                    return new ApiErrorResult<int>("Đã tồn tại mã nhân viên này trong hệ thống.");
                }
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
                var nhanViens = await _context.NhanViens.Where(m => request.Ids.Contains(m.Id)).ToListAsync();
                if (nhanViens == null) throw new GiaoHangNhanhException($"Cannot find Id: {string.Join(";", request.Ids)}");

                _context.NhanViens.RemoveRange(nhanViens);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<List<NhanVienDto>>> GetAll(ManageNhanVienPagingRequest request)
        {
            var query = from nv in _context.NhanViens
                        join g in _context.Genders on nv.GenderId equals g.Id
                        join bc in _context.BuuCucs on nv.BuuCucLamViecId equals bc.Id
                        join kv in _context.KhuVucs on bc.KhuVucId equals kv.Id
                        join h in _context.Huyens on kv.HuyenId equals h.Id
                        join t in _context.Tinhs on h.TinhId equals t.Id
                        select new { nv, g, bc, kv, h, t };

            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.nv.FirstName.Contains(request.TextSearch));
            }

            return new ApiSuccessResult<List<NhanVienDto>>(await query.Select(x => new NhanVienDto()
            {
                Id = x.nv.Id,
                Code = x.nv.Code,
                FullName = $"{x.nv.LastName} {x.nv.FirstName}",
                DiaChi = x.nv.DiaChi,
                SoDienThoai = x.nv.SoDienThoai,
                Gender = new GenderDto()
                {
                    Id = x.g.Id,
                    Name = x.g.Name
                },
                BuuCuc = new BuuCucDto()
                {
                    Id = x.bc.Id,
                    Name = x.bc.Name
                }
            }).AsNoTracking().ToListAsync());
        }

        public async Task<ApiResult<NhanVienDto>> GetById(int id)
        {
            var nhanVien = await (from nv in _context.NhanViens
                                  join g in _context.Genders on nv.GenderId equals g.Id
                                  join bc in _context.BuuCucs on nv.BuuCucLamViecId equals bc.Id
                                  join kv in _context.KhuVucs on bc.KhuVucId equals kv.Id
                                  join h in _context.Huyens on kv.HuyenId equals h.Id
                                  join t in _context.Tinhs on h.TinhId equals t.Id
                                  where nv.Id == id
                                  select new NhanVienDto()
                                  {
                                      Id = nv.Id,
                                      Code = nv.Code,
                                      CCCD = nv.CCCD,
                                      FirstName = nv.FirstName,
                                      LastName = nv.LastName,
                                      DiaChi = nv.DiaChi,
                                      Email = nv.Email,
                                      NgaySinh = nv.NgaySinh,
                                      SoDienThoai = nv.SoDienThoai,
                                      NgayLamViec = nv.NgayLamViec,
                                      NgayNghiViec = nv.NgayNghiViec != null ? nv.NgayNghiViec.Value.ToString("dd/MM/yyyy") : string.Empty,
                                      NoiSinh = nv.NoiSinh,
                                      IsActive = nv.IsActive,
                                      Gender = new GenderDto()
                                      {
                                          Id = g.Id,
                                          Name = g.Name
                                      },
                                      BuuCuc = new BuuCucDto()
                                      {
                                          Id = bc.Id,
                                          Name = bc.Name
                                      },
                                      KhuVuc = new KhuVucDto()
                                      {
                                          Id = kv.Id,
                                          Name = kv.Name
                                      },
                                      Huyen = new HuyenDto()
                                      {
                                          Id = h.Id,
                                          Name = h.Name
                                      },
                                      Tinh = new TinhDto()
                                      {
                                          Id = t.Id,
                                          Name = t.Name
                                      }

                                  }).AsNoTracking().FirstOrDefaultAsync();
            return new ApiSuccessResult<NhanVienDto>(nhanVien);
        }

        public async Task<PagedResult<NhanVienDto>> GetManageListPaging(ManageNhanVienPagingRequest request)
        {
            var query = from nv in _context.NhanViens
                        join g in _context.Genders on nv.GenderId equals g.Id
                        join bc in _context.BuuCucs on nv.BuuCucLamViecId equals bc.Id
                        join kv in _context.KhuVucs on bc.KhuVucId equals kv.Id
                        join h in _context.Huyens on kv.HuyenId equals h.Id
                        join t in _context.Tinhs on h.TinhId equals t.Id
                        select new { nv, g, bc, kv, h, t };
            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
            {
                query = query.Where(x => x.nv.Code.Contains(request.TextSearch));
                query = query.Where(x => x.nv.FirstName.Contains(request.TextSearch));
            }

            if (request.FilterByBuuCucLamViecId != null)
            {
                query = query.Where(x => x.bc.Id == request.FilterByBuuCucLamViecId.Value);
            }

            if (request.FilterByGenderId != null)
            {
                query = query.Where(x => x.g.Id == request.FilterByGenderId.Value);
            }
            //3.Sort
            if (!string.IsNullOrEmpty(request.OrderCol))
            {
                switch (request.OrderCol)
                {

                    case "code":
                        query = (request.OrderDir == "asc") ?
                            query.OrderBy(x => x.nv.Code) :
                            query.OrderByDescending(x => x.nv.Code);

                        break;

                    case "firstName":
                        query = (request.OrderDir == "asc") ? query.OrderBy(x => x.nv.Name) :
                            query.OrderByDescending(x => x.nv.Name);

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
            var data = await query.Select(x => new NhanVienDto()
            {
                Id = x.nv.Id,
                Code = x.nv.Code,
                FullName = $"{x.nv.LastName} {x.nv.FirstName}",
                DiaChi = x.nv.DiaChi,
                SoDienThoai = x.nv.SoDienThoai,
                Gender = new GenderDto()
                {
                    Id = x.g.Id,
                    Name = x.g.Name
                },
                BuuCuc = new BuuCucDto()
                {
                    Id = x.bc.Id,
                    Name = x.bc.Name
                }
            }).AsNoTracking().ToListAsync();

            //5. Select and projection
            var pagedResult = new PagedResult<NhanVienDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(NhanVienRequest request)
        {
            try
            {
                var nhanVien = await _context.NhanViens.FindAsync(request.Id);
                DateTime ngayValue;
                if (nhanVien == null) throw new GiaoHangNhanhException($"Không tìm thấy nhân viên với id: {request.Id}");

                if (nhanVien.Code != null)
                {

                    if (!await ExistByCode(request.Id, request.Code))
                    {
                        nhanVien.Code = request.Code;
                    }
                    else
                    {
                        return new ApiErrorResult<int>("Đã tồn tại mã nhân viên này trong hệ thống.");
                    }
                }



                nhanVien.FirstName = request.FirstName;
                nhanVien.LastName = request.LastName;
                nhanVien.CCCD = request.CCCD;
                nhanVien.DiaChi = request.DiaChi;
                nhanVien.BuuCucLamViecId = request.BuuCucLamViecId;
                nhanVien.SoDienThoai = request.SoDienThoai;
                nhanVien.Email = request.Email;
                nhanVien.NgaySinh = (DateTime.TryParseExact(request.NgaySinh, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>());
                nhanVien.NgayLamViec = (DateTime.TryParseExact(request.NgayLamViec, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>());
                nhanVien.NgayNghiViec = (DateTime.TryParseExact(request.NgayNghiViec, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>());
                nhanVien.NoiSinh = request.NoiSinh;
                nhanVien.GenderId = request.GenderId;
                nhanVien.IsActive = request.IsActive;
                nhanVien.ModifiedDate = DateTime.Now;
                nhanVien.ModifiedUserId = request.ModifiedUserId;
                _context.Update(nhanVien);



                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }
        public async Task<bool> ExistByCode(int? id, string code)
        {
            NhanVien nhanVien = null;
            if (id.HasValue)
            {
                nhanVien = await _context.NhanViens.Where(x => x.Code.Trim().ToLower() == code.Trim().ToLower() && x.Id != id.Value).FirstOrDefaultAsync();
            }
            else
            {
                nhanVien = await _context.NhanViens.Where(x => x.Code.Trim().ToLower() == code.Trim().ToLower()).FirstOrDefaultAsync();
            }

            return nhanVien != null;
        }


    }
}
