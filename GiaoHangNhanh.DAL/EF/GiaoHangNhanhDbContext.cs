using GiaoHangNhanh.DAL.Configurations;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace GiaoHangNhanh.DAL.EF
{
    public class GiaoHangNhanhDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public GiaoHangNhanhDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //configuration using Fluent API
            //User,Role Claimns
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            //User,Role Identity
            modelBuilder.ApplyConfiguration(new AppConfigConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());

            //Catalog
            modelBuilder.ApplyConfiguration(new BuuCucConfiguration());
            modelBuilder.ApplyConfiguration(new CongTyGuiHangConfiguration());
            modelBuilder.ApplyConfiguration(new DichVuConfiguration());
            modelBuilder.ApplyConfiguration(new HuyenConfiguration());
            modelBuilder.ApplyConfiguration(new KhuVucConfiguration());
            modelBuilder.ApplyConfiguration(new PhanLoaiHangBatThuongConfiguration());
            modelBuilder.ApplyConfiguration(new PhuongThucThanhToanConfiguration());
            modelBuilder.ApplyConfiguration(new TinhConfiguration());
            modelBuilder.ApplyConfiguration(new NhanVienConfiguration());
            modelBuilder.ApplyConfiguration(new GenderConfiguration());

            //Manipulation
            modelBuilder.ApplyConfiguration(new DangKyChuyenHoanConfiguration());
            modelBuilder.ApplyConfiguration(new DangKyKienVanDeConfiguration());
            modelBuilder.ApplyConfiguration(new KyNhanConfiguration());
            modelBuilder.ApplyConfiguration(new KhachHangConfiguration());
            modelBuilder.ApplyConfiguration(new LichSuBaoHangConfiguration());
            modelBuilder.ApplyConfiguration(new LichSuChuyenHangConfiguration());
            modelBuilder.ApplyConfiguration(new LichSuGuiHangConfiguration());
            modelBuilder.ApplyConfiguration(new LichSuNhapKhoConfigutaion());
            modelBuilder.ApplyConfiguration(new LichSuHangDenConfiguration());

            //modelBuilder.ApplyConfiguration(new PhanLoaiVanDonConfiguration());

            //Vận đơn
            modelBuilder.ApplyConfiguration(new VanDonConfiguration());



            //Menu
            modelBuilder.ApplyConfiguration(new MenuConfiguration());
            modelBuilder.ApplyConfiguration(new MenuAppRoleConfiguration());

            modelBuilder.Seed();
            //base.OnModelCreating(builder);
        }
        public DbSet<AppConfig> AppConfigs { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<BuuCuc> BuuCucs { get; set; }
        public DbSet<CongTyGuiHang> CongTyGuiHangs { get; set; }
        public DbSet<DangKyChuyenHoan> DangKyChuyenHoans { get; set; }
        public DbSet<DangKyKienVanDe> DangKyKienVanDes { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<Huyen> Huyens { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<KhuVuc> KhuVucs { get; set; }
        public DbSet<KyNhan> KyNhans { get; set; }
        public DbSet<LichSuBaoHang> LichSuBaoHangs { get; set; }
        public DbSet<LichSuChuyenHang> LichSuChuyenHangs { get; set; }
        public DbSet<LichSuHangDen> LichSuHangDens { get; set; }
        public DbSet<LichSuGuiHang> LichSuGuiHangs { get; set; }
        public DbSet<LichSuNhapKho> LichSuNhapKhos { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuAppRole> MenuAppRoles { get; set; }
        public DbSet<PhanLoaiHangBatThuong> PhanLoaiHangBatThuongs { get; set; }
        public DbSet<PhanLoaiVanDon> PhanLoaiVanDons { get; set; }
        public DbSet<PhuongThucThanhToan> PhuongThucThanhToans { get; set; }
        public DbSet<Tinh> Tinhs { get; set; }
        public DbSet<VanDon> VanDons { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }

    }
}
