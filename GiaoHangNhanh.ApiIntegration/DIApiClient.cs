﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GiaoHangNhanh.ApiIntegration
{
    public static class DIApiClient
    {
        public static IServiceCollection AddDIApiClient(this IServiceCollection services)
        {
            services.AddTransient<IAdminAppUIApiClient, AdminAppUIApiCLient>();
            services.AddTransient<IAppRoleApiClient, AppRoleApiClient>();
            services.AddTransient<IBuuCucApiClient, BuuCucApiClient>();
            services.AddTransient<ICongTyGuiHangApiClient, CongTyGuiHangApiClient>();
            services.AddTransient<IDichVuApiClient, DichVuApiClient>();
            services.AddTransient<IHuyenApiClient, HuyenApiClient>();
            services.AddTransient<IKhuVucApiClient, KhuVucApiClient>();
            services.AddSingleton<IMenuApiClient, MenuApiClient>();
            services.AddSingleton<IMenuAppRoleApiClient, MenuAppRoleApiClient>();
            services.AddTransient<IPhanLoaiHangBatThuongApiClient, PhanLoaiHangBatThuongApiClient>();
            services.AddTransient<IPhuongThucThanhToanApiClient, PhuongThucThanhToanApiClient>();
            services.AddTransient<ITinhApiClient, TinhApiClient>();
            services.AddTransient<IUserApiClient, UserApiClient>();


            //
            services.AddTransient<ILichSuNhapKhoApiClient, LichSuNhapKhoApiClient>();
            services.AddTransient<ILichSuThaoBaoApiClient, LichSuThaoBaoApiClient>();
            services.AddTransient<ILichSuDongBaoApiClient, LichSuDongBaoApiClient>();
            services.AddTransient<ILichSuGuiHangApiClient, LichSuGuiHangApiClient>();
            services.AddTransient<ILichSuChuyenHangApiClient, LichSuChuyenHangApiClient>();
            services.AddTransient<ILichSuBaoHangApiClient, LichSuBaoHangApiClient>();
            services.AddTransient<IDangKyKienVanDeApiClient, DangKyKienVanDeApiClient>();
            services.AddTransient<IDangKyChuyenHoanApiClient, DangKyChuyenHoanApiClient>();
            services.AddTransient<IVanDonApiClient, VanDonApiClient>();
            services.AddTransient<ILichSuXeDiApiClient, LichSuXeDiApiClient>();
            services.AddTransient<ILichSuXeDenApiClient, LichSuXeDenApiClient>();


            return services;
        }
    }
}
