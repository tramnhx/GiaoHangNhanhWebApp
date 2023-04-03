using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.Services.Catalog;
using GiaoHangNhanh.Services.Common;
using GiaoHangNhanh.Services.Manipulation;
using GiaoHangNhanh.Services.System;
using GiaoHangNhanh.Services.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace GiaoHangNhanh.Services
{
    public static class DIService
    {
        public static IServiceCollection AddDIService(this IServiceCollection services)
        {
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();


           
            //Catalog
            services.AddTransient<IBuuCucService, BuuCucService>();
            services.AddTransient<IDichVuService, DichVuService>();
            services.AddTransient<IPhanLoaiHangBatThuongService, PhanLoaiHangBatThuongService>();
            services.AddTransient<IHuyenService, HuyenService>();
            services.AddTransient<IGenderService, GenderService>();

            services.AddTransient<IPhuongThucThanhToanService, PhuongThucThanhToanService>();
            services.AddTransient<IKhuVucService, KhuVucService>();
            services.AddTransient<ITinhService, TinhService>();
            services.AddTransient<ICongTyGuiHangService, CongTyGuiHangService>();

            services.AddTransient<IKhachHangService, KhachHangService>();

            //Common
            services.AddTransient<IFileStorageService, FileStorageService>();

            //Manipulation
            services.AddTransient<IPhanLoaiVanDonService, PhanLoaiVanDonService>();
            services.AddTransient<IKyNhanService, KyNhanService>();
            services.AddTransient<ILichSuNhapKhoService, LichSuNhapKhoService>();
            services.AddTransient<ILichSuGuiHangService, LichSuGuiHangService>();
            services.AddTransient<ILichSuHangDenService, LichSuHangDenService>();
            services.AddTransient<IDangKyChuyenHoanService, DangKyChuyenHoanService>();
            services.AddTransient<IDuyetChuyenHoanService, DuyetChuyenHoanService>();
            services.AddTransient<IDangKyKienVanDeService, DangKyKienVanDeService>();
            services.AddTransient<ILichSuXeDiService, LichSuXeDiService>();
            services.AddTransient<ILichSuXeDenService, LichSuXeDenService>();
            services.AddTransient<ILichSuPhatHangService, LichSuPhatHangService>();
            services.AddTransient<IVanDonService, VanDonService>();
            services.AddTransient<ILichSuBaoHangService, LichSuBaoHangService>();

            //UI
            services.AddTransient<IAdminAppUIService, AdminAppUIService>();

            //System
            services.AddTransient<IAppRoleService, AppRoleService>();
            services.AddTransient<IMenuAppRoleService, MenuAppRoleService>();
            services.AddTransient<IMenuService, MenuService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<INhanVienService, NhanVienService>();


            return services;
        }
    }
}
