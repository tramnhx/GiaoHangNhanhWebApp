using GiaoHangNhanh.DAL.Entities.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace GiaoHangNhanh.DAL.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            //AppRoles
            var storeManagerRoleId = new Guid("8D04DCE2-969A-435D-BBA4-DF3F325983DC");
            var generalManagerRoleId = new Guid("8D04DCE2-969A-435D-BBA5-DF3F325983DC");
            var warehouseStaffRoleId = new Guid("8D04DCE2-969A-435D-BBA6-DF3F325983DC");
            var cashierRoleId = new Guid("8D04DCE2-969A-435D-BBA7-DF3F325983DC");
            var shopAssistantRoleId = new Guid("8D04DCE2-969A-435D-BBA8-DF3F325983DC");
            var branchManagerRoleId = new Guid("8D04DCE2-969A-435D-BBA9-DF3F325983DC");
            var administratorRoleId = new Guid("8D04DCE2-969A-435D-BBA3-DF3F325983DC");

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = storeManagerRoleId, Name = "PostOfficeManager", NormalizedName = "PostOfficeManagerManager", ChucDanh = "Quản lý bưu cục", Description = "Quản lý bưu cục" },
                new AppRole { Id = generalManagerRoleId, Name = "Manager", NormalizedName = "manager", ChucDanh = "Quản lý", Description = "Quản lý" },
                new AppRole { Id = warehouseStaffRoleId, Name = "WarehouseStaff", NormalizedName = "WarehouseStaff", ChucDanh = "Nhân viên kho", Description = "Nhân viên kho" },
                new AppRole { Id = cashierRoleId, Name = "CustomerReceptionStaff", NormalizedName = "CustomerReceptionStaff", ChucDanh = "Nhân viên tiếp nhận khách hàng", Description = "Nhân viên tiếp nhận khách hàng" },
                new AppRole { Id = shopAssistantRoleId, Name = "Shipper", NormalizedName = "shipper", ChucDanh = "Nhân viên giao hàng", Description = "Nhân viên giao hàng" },
                new AppRole { Id = branchManagerRoleId, Name = "BranchManager", NormalizedName = "BranchManager", ChucDanh = "Quản lý chi nhánh", Description = "Quản lý chi nhánh" },
                new AppRole { Id = administratorRoleId, Name = "Admin", NormalizedName = "admin", ChucDanh = "Quản trị hệ thống", Description = "Quản trị hệ thống" }
            );
            // AppConfig
            modelBuilder.Entity<AppConfig>().HasData(
                new AppConfig() { Key = "HomeTitle", Value = "This is home page of GiaoHangNhanh" },
                new AppConfig() { Key = "HomeKeyword", Value = "This is keyword of GiaoHangNhanh" },
                new AppConfig() { Key = "HomeDescription", Value = "This is Description of GiaoHangNhanh" }
                );
            // identity user admin 
            var ADMIN_ID = new Guid("447FE343-9985-412C-BB19-C6F398BC014F");
            var ROLE_ID = new Guid("38A2E063-1B9F-48D0-B7F8-55F2E835E8F0");
            // any guid, but nothing is against to use the same one
            modelBuilder.Entity<AppRole>().HasData(new AppRole
            {
                Id = ROLE_ID,
                Name = "admin",
                NormalizedName = "admin",
                Description = "Administrator role"
            });

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = ADMIN_ID,
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "admin@gmail.com",
                NormalizedEmail = "admin@gmail.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "asd123"),
                SecurityStamp = string.Empty,
                FirstName = "Tram",
                LastName = "Nguyen",
                Dob = new DateTime(2022, 10, 04),
                IsActive = true,
                MaNhanVien = $"NV{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}{DateTime.Now.Second}"
            });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
            //Menus
            modelBuilder.Entity<Menu>().HasData(
                //new Menu() { Id = 1, SortOrder = 1, Code = "TT", Name = "Thao tác", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                //new Menu() { Id = 2, SortOrder = 1, Code = "TT_NH", Name = "Nhận hàng", ParentId = 1, Link = "/NhanHang/Index", ControllerName = "NhanHang", ActionName = "Index" },
                //new Menu() { Id = 3, SortOrder = 2, Code = "TT_GH", Name = "Gửi hàng", ParentId = 1, Link = "/LichSuGuiHang/Index", ControllerName = "LichSuGuiHang", ActionName = "Index" },
                //new Menu() { Id = 4, SortOrder = 3, Code = "TT_HD", Name = "Hàng đến", ParentId = 1, Link = "/LichSuHangDen/Index", ControllerName = "LichSuHangDen", ActionName = "Index" },
                //new Menu() { Id = 5, SortOrder = 4, Code = "TT_PH", Name = "Phát hàng", ParentId = 1, Link = "/PhatHang/Index", ControllerName = "PhatHang", ActionName = "Index" },
                //new Menu() { Id = 6, SortOrder = 5, Code = "TT_HBT", Name = "Hàng bất thường", ParentId = 1, Link = "/HangBatThuong/Index", ControllerName = "HangBatThuong", ActionName = "Index" },
                //new Menu() { Id = 7, SortOrder = 6, Code = "TT_HLK", Name = "Hàng lưu kho", ParentId = 1, Link = "/HangLuuKho/Index", ControllerName = "HangLuuKho", ActionName = "Index" },
                //new Menu() { Id = 8, SortOrder = 7, Code = "TT_NK", Name = "Nhập kho", ParentId = 1, Link = "/LichSuNhapKho/Index", ControllerName = "LichSuNhapKho", ActionName = "Index" },
                //new Menu() { Id = 9, SortOrder = 8, Code = "TT_DB", Name = "Đóng bao", ParentId = 1, Link = "/DongBaoHang/Index", ControllerName = "DongBaoHang", ActionName = "Index" },
                //new Menu() { Id = 10, SortOrder = 9, Code = "TT_TB", Name = "Tháo bao", ParentId = 1, Link = "/ThaoBaoHang/Index", ControllerName = "ThaoBaoHang", ActionName = "Index" },
                //new Menu() { Id = 11, SortOrder = 10, Code = "TT_CH", Name = "Chuyển hàng", ParentId = 1, Link = "/LichSuChuyenHang/Index", ControllerName = "LichSuChuyenHang", ActionName = "Index" },
                //new Menu() { Id = 12, SortOrder = 11, Code = "TT_Xde", Name = "Xe đến", ParentId = 1, Link = "/LichSuXeDen/Index", ControllerName = "LichSuXeDen", ActionName = "Index" },
                //new Menu() { Id = 17, SortOrder = 2, Code = "VD", Name = "Vận đơn", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                //new Menu() { Id = 18, SortOrder = 1, Code = "VD_TVD", Name = "Tra vận đơn", ParentId = 17, Link = "/VanDon/Index", ControllerName = "VanDon", ActionName = "Index" },
                //new Menu() { Id = 21, SortOrder = 4, Code = "KN", Name = "Ký nhận", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                //new Menu() { Id = 22, SortOrder = 1, Code = "KN_KN", Name = "Ký nhận", ParentId = 21, Link = "/KyNhan/Index", ControllerName = "KyNhan", ActionName = "Index" },
                //new Menu() { Id = 23, SortOrder = 5, Code = "KVD", Name = "Kiện vấn đề", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                //new Menu() { Id = 24, SortOrder = 1, Code = "KVD_KVD", Name = "Kiện vấn đề", ParentId = 23, Link = "/KienVanDe/Index", ControllerName = "KienVanDe", ActionName = "Index" },
                //new Menu() { Id = 27, SortOrder = 7, Code = "XLCT", Name = "Xử lý chuyển tiếp", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                //new Menu() { Id = 28, SortOrder = 1, Code = "XLCT_XLCT", Name = "Xử lý chuyển tiếp", ParentId = 27, Link = "/XuLyChuyenTiep/Index", ControllerName = "XuLyChuyenTiep", ActionName = "Index" },
                //new Menu() { Id = 29, SortOrder = 8, Code = "DLCB", Name = "Dữ liệu cơ bản", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                //new Menu() { Id = 30, SortOrder = 1, Code = "DLCB_PTTT", Name = "Phương thức thanh toán", ParentId = 29, Link = "/PhuongThucThanhToan/Index", ControllerName = "PhuongThucThanhToan", ActionName = "Index" },
                //new Menu() { Id = 31, SortOrder = 2, Code = "DLCB_PLHBT", Name = "Phân loại hàng bất thường", ParentId = 29, Link = "/PhanLoaiHangBatThuong/Index", ControllerName = "PhanLoaiHangBatThuong", ActionName = "Index" },
                //new Menu() { Id = 32, SortOrder = 3, Code = "DLCB_DV", Name = "Dịch vụ", ParentId = 29, Link = "/DichVu/Index", ControllerName = "DichVu", ActionName = "Index" },
                //new Menu() { Id = 33, SortOrder = 4, Code = "DLCB_T", Name = "Tỉnh", ParentId = 29, Link = "/Tinh/Index", ControllerName = "Tinh", ActionName = "Index" },
                //new Menu() { Id = 34, SortOrder = 5, Code = "DLCB_H", Name = "Huyện", ParentId = 29, Link = "/Huyen/Index", ControllerName = "Huyen", ActionName = "Index" },
                //new Menu() { Id = 35, SortOrder = 6, Code = "DLCB_KV", Name = "Khu Vực", ParentId = 29, Link = "/KhuVuc/Index", ControllerName = "KhuVuc", ActionName = "Index" },
                //new Menu() { Id = 36, SortOrder = 7, Code = "DLCB_BC", Name = "Bưu cục", ParentId = 29, Link = "/BuuCuc/Index", ControllerName = "BuuCuc", ActionName = "Index" },
                //new Menu() { Id = 37, SortOrder = 9, Code = "KH", Name = "Khách hàng", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns = 'http://www.w3.org/2000/svg' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><path d = 'M18,14 C16.3431458,14 15,12.6568542 15,11 C15,9.34314575 16.3431458,8 18,8 C19.6568542,8 21,9.34314575 21,11 C21,12.6568542 19.6568542,14 18,14 Z M9,11 C6.790861,11 5,9.209139 5,7 C5,4.790861 6.790861,3 9,3 C11.209139,3 13,4.790861 13,7 C13,9.209139 11.209139,11 9,11 Z' fill='#000000' fill-rule='nonzero' opacity='0.3'></path><path d = 'M17.6011961,15.0006174 C21.0077043,15.0378534 23.7891749,16.7601418 23.9984937,20.4 C24.0069246,20.5466056 23.9984937,21 23.4559499,21 L19.6,21 C19.6,18.7490654 18.8562935,16.6718327 17.6011961,15.0006174 Z M0.00065168429,20.1992055 C0.388258525,15.4265159 4.26191235,13 8.98334134,13 C13.7712164,13 17.7048837,15.2931929 17.9979143,20.2 C18.0095879,20.3954741 17.9979143,21 17.2466999,21 C13.541124,21 8.03472472,21 0.727502227,21 C0.476712155,21 -0.0204617505,20.45918 0.00065168429,20.1992055 Z' fill='#000000' fill-rule='nonzero'></path></svg></span></span>" },
                //new Menu() { Id = 38, SortOrder = 1, Code = "KH_KH", Name = "Khách hàng", ParentId = 37, Link = "/KhachHang/Index", ControllerName = "KhachHang", ActionName = "Index" },
                //new Menu() { Id = 39, SortOrder = 2, Code = "KH_CTGH", Name = "Công ty gửi hàng", ParentId = 37, Link = "/CongTyGuiHang/Index", ControllerName = "CongTyGuiHang", ActionName = "Index" },
                //new Menu() { Id = 54, SortOrder = 17, Code = "QTHT", Name = "Quản trị hệ thống", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24' fill='none'><path opacity='0.25' d='M2 6.5C2 4.01472 4.01472 2 6.5 2H17.5C19.9853 2 22 4.01472 22 6.5V6.5C22 8.98528 19.9853 11 17.5 11H6.5C4.01472 11 2 8.98528 2 6.5V6.5Z' fill='#12131A'></path><path d='M20 6.5C20 7.88071 18.8807 9 17.5 9C16.1193 9 15 7.88071 15 6.5C15 5.11929 16.1193 4 17.5 4C18.8807 4 20 5.11929 20 6.5Z' fill='#12131A'></path><path opacity='0.25' d='M2 17.5C2 15.0147 4.01472 13 6.5 13H17.5C19.9853 13 22 15.0147 22 17.5V17.5C22 19.9853 19.9853 22 17.5 22H6.5C4.01472 22 2 19.9853 2 17.5V17.5Z' fill='#12131A'></path><path d='M9 17.5C9 18.8807 7.88071 20 6.5 20C5.11929 20 4 18.8807 4 17.5C4 16.1193 5.11929 15 6.5 15C7.88071 15 9 16.1193 9 17.5Z' fill='#12131A'></path></svg></span></span>" },
                //new Menu() { Id = 55, SortOrder = 1, Code = "QTHT_U", Name = "Người dùng", ParentId = 54, Link = "/Staff/Index", ControllerName = "Staff", ActionName = "Index" },
                //new Menu() { Id = 56, SortOrder = 2, Code = "QTHT_NV", Name = "Nhân viên", ParentId = 54, Link = "/NhanVien/Index", ControllerName = "NhanVien", ActionName = "Index" },
                //new Menu() { Id = 57, SortOrder = 3, Code = "QTHT_PQ", Name = "Phân quyền", ParentId = 54, Link = "/AppRole/Index", ControllerName = "AppRole", ActionName = "Index" },
                //new Menu() { Id = 58, SortOrder = 4, Code = "QTHT_DMHT", Name = "Danh mục hệ thống", ParentId = 54, Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                //new Menu() { Id = 59, SortOrder = 1, Code = "QTHT_DMHT_TTND", Name = "Trạng thái người dùng", ParentId = 58, Link = "/AppUserStatus/Index", ControllerName = "AppUserStatus", ActionName = "Index" },
                //new Menu() { Id = 60, SortOrder = 2, Code = "QTHT_DMHT_GT", Name = "Giới tính", ParentId = 58, Link = "/Gender/Index", ControllerName = "Gender", ActionName = "Index" },
                //new Menu() { Id = 61, SortOrder = 3, Code = "QTHT_DMHT_M", Name = "Menu", ParentId = 58, Link = "/Menu/Index", ControllerName = "Menu", ActionName = "Index" }

                new Menu() { Id = 1, SortOrder = 1, IsDeleted = false, IsActive = true, Code = "KH", Name = "Khách hàng", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns = 'http://www.w3.org/2000/svg' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><path d = 'M18,14 C16.3431458,14 15,12.6568542 15,11 C15,9.34314575 16.3431458,8 18,8 C19.6568542,8 21,9.34314575 21,11 C21,12.6568542 19.6568542,14 18,14 Z M9,11 C6.790861,11 5,9.209139 5,7 C5,4.790861 6.790861,3 9,3 C11.209139,3 13,4.790861 13,7 C13,9.209139 11.209139,11 9,11 Z' fill='#000000' fill-rule='nonzero' opacity='0.3'></path><path d = 'M17.6011961,15.0006174 C21.0077043,15.0378534 23.7891749,16.7601418 23.9984937,20.4 C24.0069246,20.5466056 23.9984937,21 23.4559499,21 L19.6,21 C19.6,18.7490654 18.8562935,16.6718327 17.6011961,15.0006174 Z M0.00065168429,20.1992055 C0.388258525,15.4265159 4.26191235,13 8.98334134,13 C13.7712164,13 17.7048837,15.2931929 17.9979143,20.2 C18.0095879,20.3954741 17.9979143,21 17.2466999,21 C13.541124,21 8.03472472,21 0.727502227,21 C0.476712155,21 -0.0204617505,20.45918 0.00065168429,20.1992055 Z' fill='#000000' fill-rule='nonzero'></path></svg></span></span>" },
                new Menu() { Id = 2, SortOrder = 1, IsDeleted = false, IsActive = true, Code = "KH_KH", Name = "Khách hàng", ParentId = 1, Link = "/KhachHang/Index", ControllerName = "KhachHang", ActionName = "Index" },

                new Menu() { Id = 3, SortOrder = 2, IsDeleted = false, IsActive = true, Code = "VD", Name = "Vận đơn", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                new Menu() { Id = 4, SortOrder = 1, IsDeleted = false, IsActive = true, Code = "VD_TVD", Name = "Tra vận đơn", ParentId = 3, Link = "/VanDon/Index", ControllerName = "VanDon", ActionName = "Index" },

                new Menu() { Id = 5, SortOrder = 3, IsDeleted = false, IsActive = true, Code = "TT", Name = "Thao Tác", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                new Menu() { Id = 6, SortOrder = 1, IsDeleted = false, IsActive = true, Code = "TT_KN", Name = "Ký nhận", ParentId = 5, Link = "/KyNhan/Index", ControllerName = "KyNhan", ActionName = "Index" },
                new Menu() { Id = 7, SortOrder = 2, IsDeleted = false, IsActive = true, Code = "TT_PLHBT", Name = "Phân loại hàng bất thường", ParentId = 5, Link = "/PhanLoaiHangBatThuong/Index", ControllerName = "PhanLoaiHangBatThuong", ActionName = "Index" },
                new Menu() { Id = 8, SortOrder = 3, IsDeleted = false, IsActive = true, Code = "TT_LSNK", Name = "Lịch sử nhập kho", ParentId = 5, Link = "/LichSuNhapKho/Index", ControllerName = "LichSuNhapKho", ActionName = "Index" },
                new Menu() { Id = 9, SortOrder = 4, IsDeleted = false, IsActive = true, Code = "TT_LSDB", Name = "Lịch sử đóng bao", ParentId = 5, Link = "/LichSuDongBao/Index", ControllerName = "LichSuDongBao", ActionName = "Index" },
                new Menu() { Id = 10, SortOrder = 5, IsDeleted = false, IsActive = true, Code = "TT_LSTB", Name = "Lịch sử tháo bao", ParentId = 5, Link = "/LichSuThaoBao/Index", ControllerName = "LichSuThaoBao", ActionName = "Index" },
                new Menu() { Id = 11, SortOrder = 6, IsDeleted = false, IsActive = true, Code = "TT_LSXDE", Name = "Lịch sử xe đến", ParentId = 5, Link = "/LichSuXeDen/Index", ControllerName = "LichSuXeDen", ActionName = "Index" },
                new Menu() { Id = 12, SortOrder = 7, IsDeleted = false, IsActive = true, Code = "TT_LSXD", Name = "Lịch sử xe đến", ParentId = 5, Link = "/LichSuXeDi/Index", ControllerName = "LichSuXeDi", ActionName = "Index" },
                new Menu() { Id = 13, SortOrder = 8, IsDeleted = false, IsActive = true, Code = "TT_LSGH", Name = "Lịch sử gửi hàng", ParentId = 5, Link = "/LichSuGuiHang/Index", ControllerName = "LichSuGuiHang", ActionName = "Index" },
                
                new Menu() { Id = 14, SortOrder = 4, IsDeleted = false, IsActive = true, Code = "DM", Name = "Danh mục", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>" },
                new Menu() { Id = 15, SortOrder = 1, IsDeleted = false, IsActive = true, Code = "DM_DV", Name = "Dịch vụ", ParentId = 14, Link = "/DichVu/Index", ControllerName = "DanhMucDichVu", ActionName = "Index" },
                new Menu() { Id = 16, SortOrder = 2, IsDeleted = false, IsActive = true, Code = "DM_CTGH", Name = "Công ty gửi hàng", ParentId = 14, Link = "/CongTyGuiHang/Index", ControllerName = "DanhMucCongTyGuiHang", ActionName = "Index" },
                new Menu() { Id = 17, SortOrder = 3, IsDeleted = false, IsActive = true, Code = "DM_PTTT", Name = "Phương thức thanh toán", ParentId = 14, Link = "/PhuongThucThanhToan/Index", ControllerName = "DanhMucPhuongThucThanhToan", ActionName = "Index" },
                
                new Menu() { Id = 18, SortOrder = 5, IsDeleted = false, IsActive = true, Code = "QTHT", Name = "Quản trị hệ thống", ParentId = new Nullable<int>(), Link = string.Empty, Icon = "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24' fill='none'><path opacity='0.25' d='M2 6.5C2 4.01472 4.01472 2 6.5 2H17.5C19.9853 2 22 4.01472 22 6.5V6.5C22 8.98528 19.9853 11 17.5 11H6.5C4.01472 11 2 8.98528 2 6.5V6.5Z' fill='#12131A'></path><path d='M20 6.5C20 7.88071 18.8807 9 17.5 9C16.1193 9 15 7.88071 15 6.5C15 5.11929 16.1193 4 17.5 4C18.8807 4 20 5.11929 20 6.5Z' fill='#12131A'></path><path opacity='0.25' d='M2 17.5C2 15.0147 4.01472 13 6.5 13H17.5C19.9853 13 22 15.0147 22 17.5V17.5C22 19.9853 19.9853 22 17.5 22H6.5C4.01472 22 2 19.9853 2 17.5V17.5Z' fill='#12131A'></path><path d='M9 17.5C9 18.8807 7.88071 20 6.5 20C5.11929 20 4 18.8807 4 17.5C4 16.1193 5.11929 15 6.5 15C7.88071 15 9 16.1193 9 17.5Z' fill='#12131A'></path></svg></span></span>" },
                new Menu() { Id = 19, SortOrder = 1, IsDeleted = false, IsActive = true, Code = "QTHT_AC", Name = "Account", ParentId = 18, Link = "/Staff/Index", ControllerName = "Staff", ActionName = "Index" },
                new Menu() { Id = 20, SortOrder = 2, IsDeleted = false, IsActive = true, Code = "QTHT_NV", Name = "Nhân Viên", ParentId = 18, Link = "/NhanVien/Index", ControllerName = "NhanVien", ActionName = "Index" },
                new Menu() { Id = 21, SortOrder = 3, IsDeleted = false, IsActive = true, Code = "QTHT_PQ", Name = "Phân quyền", ParentId = 18, Link = "/AppRole/Index", ControllerName = "AppRole", ActionName = "Index" },
                new Menu() { Id = 22, SortOrder = 4, IsDeleted = false, IsActive = true, Code = "QTHT_DMHT_T", Name = "Tỉnh", ParentId = 18, Link = "/Tinh/Index", ControllerName = "Tinh", ActionName = "Index" },
                new Menu() { Id = 23, SortOrder = 5, IsDeleted = false, IsActive = true, Code = "QTHT_DMHT_H", Name = "Huyện", ParentId = 18, Link = "/Huyen/Index", ControllerName = "Huyen", ActionName = "Index" },
                new Menu() { Id = 24, SortOrder = 6, IsDeleted = false, IsActive = true, Code = "QTHT_DMHT_KV", Name = "Khu vực", ParentId = 18, Link = "/KhuVuc/Index", ControllerName = "KhuVuc", ActionName = "Index" },
                new Menu() { Id = 25, SortOrder = 7, IsDeleted = false, IsActive = true, Code = "QTHT_DMHT_BC", Name = "Bưu cục", ParentId = 18, Link = "/BuuCuc/Index", ControllerName = "BuuCuc", ActionName = "Index" },

                new Menu() { Id = 26, SortOrder = 8, IsDeleted = false, IsActive = true, Code = "QTHT_DMHT_M", Name = "Menu", ParentId = 18, Link = "/Menu/Index", ControllerName = "Menu", ActionName = "Index" }
            );

            //Tinhs
            modelBuilder.Entity<Tinh>().HasData(
                new Tinh
                {
                    Id = 1,
                    Code = "KH",
                    Name = "Khánh Hòa",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                }
            );

            //DichVus
            modelBuilder.Entity<DichVu>().HasData(
                new DichVu
                {
                    Id = 1,
                    Code = "GHQT",
                    Name = "Giao hàng quốc tế",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                }
            );

            //PhuongThucThanhToans
            modelBuilder.Entity<PhuongThucThanhToan>().HasData(
                new PhuongThucThanhToan
                {
                    Id = 1,
                    Code = "TT",
                    Name = "Thanh toán trực tiếp",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                }
            );

            //PhanLoaiHangBatThuongs
            modelBuilder.Entity<PhanLoaiHangBatThuong>().HasData(
                new PhanLoaiHangBatThuong
                {
                    Id = 1,
                    Code = "KHTC",
                    Name = "Khách hàng từ chối nhận",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                }
            );

            //Huyens
            modelBuilder.Entity<Huyen>().HasData(
                new Huyen
                {
                    Id = 1,
                    Code = "NT",
                    Name = "Thành phố Nha Trang",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    TinhId = 1
                }
            );

            //KhuVucs
            modelBuilder.Entity<KhuVuc>().HasData(
                new KhuVuc
                {
                    Id = 1,
                    Code = "VN",
                    Name = "Vĩnh Ngọc",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    HuyenId = 1
                }
            );

            //BuuCucs
            modelBuilder.Entity<BuuCuc>().HasData(
                new BuuCuc
                {
                    Id = 1,
                    Code = "AL1",
                    Name = "Alpha1",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    TinhId = 1,
                    HuyenId = 1,
                    KhuVucId=1,
                }
            );

            //CongTyGuiHangs
            modelBuilder.Entity<CongTyGuiHang>().HasData(
                new CongTyGuiHang
                {
                    Id = 1,
                    Code = "SPC",
                    Name = "Shopee",
                    PhoneNumber = "0958624542",
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    TinhId = 1,
                    HuyenId = 1,
                    KhuVucId = 1,
                }
            );

            //VanDons
            modelBuilder.Entity<VanDon>().HasData(
                new VanDon
                {
                    Id = 1,
                    Code = "VDTN01CDA",
                    BuuCucHangDenId = 1,
                    CongTyGuiHangId = 1,
                    DichVuId = 1,
                    PhuongThucThanhToanId = 1,
                    COD = 12000,
                    CreatedDate = DateTime.Now,
                    CreatedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    HoTenNguoiNhan = "Ngoc Thanh",
                    DienThoaiNguoiNhan = "0905684251",
                    DiaChiNguoiNhan = "79 Mai Thi Dong, Khanh Hoa",
                    ModifiedUserId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                    TrongLuong = 0.6,
                    NgayGuiHang = DateTime.Now,
                    NhanVienLayHangId = new Guid("447FE343-9985-412C-BB19-C6F398BC014F"),
                }
            );
        }
    }
}
