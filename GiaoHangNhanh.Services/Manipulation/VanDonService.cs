using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhuongThucThanhToans;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
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
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.Manipulation
{
    public interface IVanDonService
    {
        //Task<ApiResult<List<VanDonDto>>> GetAll(ManageVanDonPagingRequest request);
        Task<ApiResult<int>> CreateAsync(VanDonRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<VanDonDto>> GetById(int? id);
        Task<ApiResult<VanDonDto>> GetByCode(string code);
        Task<PagedResult<VanDonDto>> GetManageListPaging(ManageVanDonPagingRequest request);
        Task<ApiResult<int>> UpdateAsync(VanDonRequest request);
    }
    public class VanDonService : IVanDonService
    {
        private readonly GiaoHangNhanhDbContext _context;
        private readonly IConfiguration _configuration;
        public VanDonService(GiaoHangNhanhDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<ApiResult<int>> CreateAsync(VanDonRequest request)
        {
            DateTime ngayGuiHangValue;

            try
            {
                var vanDon = new VanDon()
                {
                    Code = GenerateCode().Result,
                    BuuCucHangDenId = request.BuuCucHangDenId,
                    DichVuId = request.DichVuId,
                    CongTyGuiHangId = request.CongTyGuiHangId,
                    PhuongThucThanhToanId = request.PhuongThucThanhToanId,
                    CreatedUserId = Guid.Parse(request.CreatedUserId),
                    NhanVienId = request.NhanVienId,
                    COD = request.COD,
                    Description = request.Description,
                    DiaChiNguoiGui = request.DiaChiNguoiGui,
                    DiaChiNguoiNhan = request.DiaChiNguoiNhan,
                    DienThoaiNguoiGui = request.DienThoaiNguoiGui,
                    DienThoaiNguoiNhan = request.DienThoaiNguoiNhan,
                    HoTenNguoiGui = request.HoTenNguoiGui,
                    HoTenNguoiNhan = request.HoTenNguoiNhan,
                    TrongLuong = request.TrongLuong,
                    GiaTriHangHoa = request.GiaTriHangHoa,
                    NoiDungHangHoa = request.NoiDungHangHoa,
                    SortOrder = request.SortOrder,
                };
                if (DateTime.TryParseExact(request.NgayGuiHang, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayGuiHangValue))
                {
                    vanDon.NgayGuiHang = ngayGuiHangValue;
                }
                else
                {
                    return new ApiErrorResult<int>("Ngày gửi hảng không hợp lệ");
                }
                _context.VanDons.Add(vanDon);
                await _context.SaveChangesAsync();
                return new ApiSuccessResult<int>(vanDon.Id);
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
                var vanDons = await _context.VanDons.Where(m => request.Ids.Contains(m.Id)).AsNoTracking().ToListAsync();
                if (vanDons == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {string.Join(";", request.Ids)}");

                foreach (var item in vanDons)
                {
                    item.IsDeleted = true;
                    item.ModifiedUserId = request.DeleteUserId;
                    item.ModifiedDate = DateTime.Now;
                }
                _context.VanDons.UpdateRange(vanDons);

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<VanDonDto>> GetById(int? id)
        {
            var vanDon = await (from v in _context.VanDons
                                //where v.Id == id && v.IsDeleted == false
                                .Where(x => x.Id == id && x.IsDeleted == false)
                                join p in _context.PhuongThucThanhToans on v.PhuongThucThanhToanId equals p.Id
                                join d in _context.DichVus on v.DichVuId equals d.Id
                                join c in _context.CongTyGuiHangs on v.CongTyGuiHangId equals c.Id
                                join b in _context.BuuCucs on v.BuuCucHangDenId equals b.Id
                                join a in _context.NhanViens on v.NhanVienId equals a.Id
                                select new VanDonDto()
                                {
                                    Id = v.Id,
                                    Code = v.Code,
                                    //StrNgayGuiHang = v.NgayGuiHang.ToString("dd/MM/yyyy HH:mm:ss"),
                                    CreatedDate = v.CreatedDate,
                                    ModifiedDate = v.ModifiedDate,
                                    CreatedUserId = v.CreatedUserId,
                                    ModifiedUserId = v.ModifiedUserId,
                                    SortOrder = v.SortOrder,
                                    IsDeleted = v.IsDeleted,
                                    HoTenNguoiGui = v.HoTenNguoiGui,
                                    HoTenNguoiNhan = v.HoTenNguoiNhan,
                                    COD = v.COD,
                                    DiaChiNguoiGui = v.DiaChiNguoiGui,
                                    DiaChiNguoiNhan = v.DiaChiNguoiNhan,
                                    DienThoaiNguoiGui = v.DienThoaiNguoiGui,
                                    DienThoaiNguoiNhan = v.DienThoaiNguoiNhan,
                                    GiaTriHangHoa = v.GiaTriHangHoa,
                                    NgayGuiHang = v.NgayGuiHang,
                                    TrongLuong = v.TrongLuong,
                                    MoTaHangHoa = v.Description,
                                    NoiDungHangHoa = v.NoiDungHangHoa,
                                    CongTyGuiHang = new CongTyGuiHangDto()
                                    {
                                        Id = c.Id,
                                        Code = c.Code,
                                        Name = c.Name
                                    },
                                    DichVu = new DichVuDto()
                                    {
                                        Id = d.Id,
                                        Code = d.Code,
                                        Name = d.Name
                                    },
                                    PhuongThucThanhToan = new PhuongThucThanhToanDto()
                                    {
                                        Id = p.Id,
                                        Code = p.Code,
                                        Name = p.Name
                                    },
                                    BuuCuc = new BuuCucDto()
                                    {
                                        Id = b.Id,
                                        Code = b.Code,
                                        Name = b.Name
                                    },
                                    NhanVien = new NhanVienDto()
                                    {
                                        Id = a.Id,
                                        FullName = $"{a.LastName} {a.FirstName}"
                                    }
                                }).AsNoTracking().FirstOrDefaultAsync();

            return new ApiSuccessResult<VanDonDto>(vanDon);
        }

        public async Task<ApiResult<VanDonDto>> GetByCode(string code)
        {
            var vanDon = await (from v in _context.VanDons
                                //where v.Id == id && v.IsDeleted == false
                                .Where(x => x.Code == code && x.IsDeleted == false)
                                join p in _context.PhuongThucThanhToans on v.PhuongThucThanhToanId equals p.Id
                                join d in _context.DichVus on v.DichVuId equals d.Id
                                join c in _context.CongTyGuiHangs on v.CongTyGuiHangId equals c.Id
                                join b in _context.BuuCucs on v.BuuCucHangDenId equals b.Id
                                select new VanDonDto()
                                {
                                    Id = v.Id,
                                    Code = v.Code,
                                    CreatedDate = v.CreatedDate,
                                    ModifiedDate = v.ModifiedDate,
                                    CreatedUserId = v.CreatedUserId,

                                    ModifiedUserId = v.ModifiedUserId,
                                    SortOrder = v.SortOrder,
                                    IsDeleted = v.IsDeleted,
                                    HoTenNguoiGui = v.HoTenNguoiGui,
                                    HoTenNguoiNhan = v.HoTenNguoiNhan,
                                    COD = v.COD,
                                    DiaChiNguoiGui = v.DiaChiNguoiGui,
                                    DiaChiNguoiNhan = v.DiaChiNguoiNhan,
                                    DienThoaiNguoiGui = v.DienThoaiNguoiGui,
                                    DienThoaiNguoiNhan = v.DienThoaiNguoiNhan,
                                    GiaTriHangHoa = v.GiaTriHangHoa,
                                    NgayGuiHang = v.NgayGuiHang,
                                    TrongLuong = v.TrongLuong,
                                    MoTaHangHoa = v.Description,
                                    NoiDungHangHoa = v.NoiDungHangHoa,
                                    CongTyGuiHang = new CongTyGuiHangDto()
                                    {
                                        Id = c.Id,
                                        Code = c.Code,
                                        Name = c.Name
                                    },
                                    DichVu = new DichVuDto()
                                    {
                                        Id = d.Id,
                                        Code = d.Code,
                                        Name = d.Name
                                    },
                                    PhuongThucThanhToan = new PhuongThucThanhToanDto()
                                    {
                                        Id = p.Id,
                                        Code = p.Code,
                                        Name = p.Name
                                    },
                                    BuuCuc = new BuuCucDto()
                                    {
                                        Id = b.Id,
                                        Code = b.Code,
                                        Name = b.Name
                                    }
                                }).AsNoTracking().FirstOrDefaultAsync();

            return new ApiSuccessResult<VanDonDto>(vanDon);
        }

        public async Task<PagedResult<VanDonDto>> GetManageListPaging(ManageVanDonPagingRequest request)
        {
            //1. Selevd join
            var query = from vd in _context.VanDons
                        .Where(x => x.IsDeleted == false)
                        join p in _context.PhuongThucThanhToans on vd.PhuongThucThanhToanId equals p.Id
                        join d in _context.DichVus on vd.DichVuId equals d.Id
                        join c in _context.CongTyGuiHangs on vd.CongTyGuiHangId equals c.Id
                        join b in _context.BuuCucs on vd.BuuCucHangDenId equals b.Id
                        join a in _context.NhanViens on vd.NhanVienId equals a.Id
                        select new { vd, p, d, c, b, a };

            //2. filter
            if (!string.IsNullOrEmpty(request.TextSearch))
                query = query.Where(x => x.vd.Code.Contains(request.TextSearch));
            if (request.FilterByNhanVienId != null)
            {
                query = query.Where(x => x.vd.Id == request.FilterByNhanVienId.Value);
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            if (request.PageIndex != null)
            {
                query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                            .Take(request.PageSize);
            }
            var data = await query.Select(x => new VanDonDto()
            {
                Id = x.vd.Id,
                Code = x.vd.Code,
                CreatedDate = x.vd.CreatedDate,
                ModifiedDate = x.vd.ModifiedDate,
                CreatedUserId = x.vd.CreatedUserId,
                ModifiedUserId = x.vd.ModifiedUserId,
                StrNgayGuiHang = x.vd.NgayGuiHang.ToString("dd/MM/yyyy"),
                SortOrder = x.vd.SortOrder,
                IsDeleted = x.vd.IsDeleted,
                HoTenNguoiGui = x.vd.HoTenNguoiGui,
                HoTenNguoiNhan = x.vd.HoTenNguoiNhan,
                COD = x.vd.COD,
                DiaChiNguoiGui = x.vd.DiaChiNguoiGui,
                DiaChiNguoiNhan = x.vd.DiaChiNguoiNhan,
                DienThoaiNguoiGui = x.vd.DienThoaiNguoiGui,
                DienThoaiNguoiNhan = x.vd.DienThoaiNguoiNhan,
                GiaTriHangHoa = x.vd.GiaTriHangHoa,
                NgayGuiHang = x.vd.NgayGuiHang,
                TrongLuong = x.vd.TrongLuong,
                MoTaHangHoa = x.vd.Description,
                NoiDungHangHoa = x.vd.NoiDungHangHoa,
                CongTyGuiHang = new CongTyGuiHangDto()
                {
                    Id = x.c.Id,
                    Code = x.c.Code,
                    Name = x.c.Name
                },
                DichVu = new DichVuDto()
                {
                    Id = x.d.Id,
                    Code = x.d.Code,
                    Name = x.d.Name
                },
                PhuongThucThanhToan = new PhuongThucThanhToanDto()
                {
                    Id = x.p.Id,
                    Code = x.p.Code,
                    Name = x.p.Name
                },
                BuuCuc = new BuuCucDto()
                {
                    Id = x.b.Id,
                    Code = x.b.Code,
                    Name = x.b.Name
                },
                NhanVien = new NhanVienDto()
                {
                    Id = x.a.Id,
                    FullName = $"{x.a.LastName} {x.a.FirstName}",
                }
            }).AsNoTracking().ToListAsync();

            //4. Selevd and projevdion
            var pagedResult = new PagedResult<VanDonDto>()
            {
                TotalRecords = totalRow,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
                Items = data
            };
            return pagedResult;
        }

        public async Task<ApiResult<int>> UpdateAsync(VanDonRequest request)
        {
            DateTime ngayGuiHangValue;
            try
            {
                var vanDon = await _context.VanDons.FindAsync(request.Id);

                if (vanDon.IsDeleted == true || vanDon == null) throw new GiaoHangNhanhException($"Không tìm thấy id: {request.Id}");

                vanDon.BuuCucHangDenId = request.BuuCucHangDenId;
                vanDon.NhanVienId = request.NhanVienId;
                vanDon.DichVuId = request.DichVuId;
                vanDon.CongTyGuiHangId = request.CongTyGuiHangId;
                vanDon.PhuongThucThanhToanId = request.PhuongThucThanhToanId;
                vanDon.ModifiedUserId = Guid.Parse(request.ModifiedUserId);
                vanDon.COD = request.COD;
                vanDon.Description = request.Description;
                vanDon.DiaChiNguoiGui = request.DiaChiNguoiGui;
                vanDon.DiaChiNguoiNhan = request.DiaChiNguoiNhan;
                vanDon.DienThoaiNguoiGui = request.DienThoaiNguoiGui;
                vanDon.DienThoaiNguoiNhan = request.DienThoaiNguoiNhan;
                vanDon.HoTenNguoiGui = request.HoTenNguoiGui;
                vanDon.HoTenNguoiNhan = request.HoTenNguoiNhan;
                vanDon.TrongLuong = request.TrongLuong;
                vanDon.GiaTriHangHoa = request.GiaTriHangHoa;
                if (DateTime.TryParseExact(request.NgayGuiHang, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayGuiHangValue))
                {
                    vanDon.NgayGuiHang = ngayGuiHangValue;
                }
                else
                {
                    return new ApiErrorResult<int>("Ngày gửi hảng không hợp lệ");
                }
                vanDon.NoiDungHangHoa = request.NoiDungHangHoa;
                vanDon.ModifiedDate = DateTime.Now;
                vanDon.SortOrder = request.SortOrder;

                return new ApiSuccessResult<int>(await _context.SaveChangesAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<bool> ExistByCode(int? id, string code)
        {
            VanDon vanDon = null;
            if (id.HasValue)
            {
                vanDon = await _context.VanDons.Where(x => x.Code.Trim().ToLower() == code.Trim().ToLower()
                                                      && x.Id != id.Value).FirstOrDefaultAsync();
            }
            else
            {
                vanDon = await _context.VanDons.Where(x => x.Code.Trim().ToLower() == code.Trim().ToLower()).FirstOrDefaultAsync();
            }

            return vanDon != null;
        }

        public async Task<string> GenerateCode()
        {
            Random random = new Random();
            const string chars = "019237685932";

            string code = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());

            while (await ExistByCode(null, code))
            {
                code = new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());
            }

            return code;
        }
    }
}
