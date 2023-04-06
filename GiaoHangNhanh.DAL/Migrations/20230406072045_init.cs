﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GiaoHangNhanh.DAL.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConfigs",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfigs", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "AppRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoleClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChucDanh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgaySua = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserLogins", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaNhanVien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActivateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StartingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserTokens", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "BaoHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDongBao = table.Column<bool>(type: "bit", nullable: false),
                    MoBaoDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoHangs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DangKyChuyenHoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    NguyenNhan = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MieuTaNguyenNhan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDangKyChuyenHoan = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyChuyenHoans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucDichVus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucDichVus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucPhanLoaiHangBatThuongs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucPhanLoaiHangBatThuongs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucPhuongThucThanhToans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucPhuongThucThanhToans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucTinhs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucTinhs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KhachHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHangs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucHuyens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TinhId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucHuyens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhMucHuyens_DanhMucTinhs_TinhId",
                        column: x => x.TinhId,
                        principalTable: "DanhMucTinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuAppRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false),
                    IsAllow = table.Column<bool>(type: "bit", nullable: false),
                    MenuAppRoleType = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuAppRoles", x => new { x.AppRoleId, x.MenuId, x.Id });
                    table.ForeignKey(
                        name: "FK_MenuAppRoles_AppRoles_AppRoleId",
                        column: x => x.AppRoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuAppRoles_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucKhuVucs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HuyenId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucKhuVucs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhMucKhuVucs_DanhMucHuyens_HuyenId",
                        column: x => x.HuyenId,
                        principalTable: "DanhMucHuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucBuuCucs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TinhId = table.Column<int>(type: "int", nullable: true),
                    HuyenId = table.Column<int>(type: "int", nullable: true),
                    KhuVucId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucBuuCucs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhMucBuuCucs_DanhMucHuyens_HuyenId",
                        column: x => x.HuyenId,
                        principalTable: "DanhMucHuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhMucBuuCucs_DanhMucKhuVucs_KhuVucId",
                        column: x => x.KhuVucId,
                        principalTable: "DanhMucKhuVucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhMucBuuCucs_DanhMucTinhs_TinhId",
                        column: x => x.TinhId,
                        principalTable: "DanhMucTinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DanhMucCongTyGuiHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhId = table.Column<int>(type: "int", nullable: true),
                    HuyenId = table.Column<int>(type: "int", nullable: true),
                    KhuVucId = table.Column<int>(type: "int", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucCongTyGuiHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhMucCongTyGuiHangs_DanhMucHuyens_HuyenId",
                        column: x => x.HuyenId,
                        principalTable: "DanhMucHuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhMucCongTyGuiHangs_DanhMucKhuVucs_KhuVucId",
                        column: x => x.KhuVucId,
                        principalTable: "DanhMucKhuVucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DanhMucCongTyGuiHangs_DanhMucTinhs_TinhId",
                        column: x => x.TinhId,
                        principalTable: "DanhMucTinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KyNhans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    NhanVienPhat = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenNguoiKy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DauKyThay = table.Column<bool>(type: "bit", nullable: false),
                    NgayKyNhan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuuCucId = table.Column<int>(type: "int", nullable: true),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyNhans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KyNhans_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KyNhans_DanhMucBuuCucs_BuuCucId",
                        column: x => x.BuuCucId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuGuiHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    TramSauId = table.Column<int>(type: "int", nullable: true),
                    BuuCucId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuGuiHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuGuiHangs_DanhMucBuuCucs_BuuCucId",
                        column: x => x.BuuCucId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuNhapKhos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuuCucGuiHangId = table.Column<int>(type: "int", nullable: true),
                    VanDonId = table.Column<int>(type: "int", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BuuCucId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuNhapKhos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuNhapKhos_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuNhapKhos_DanhMucBuuCucs_BuuCucId",
                        column: x => x.BuuCucId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NhanViens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BuuCucLamViecId = table.Column<int>(type: "int", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayLamViec = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NgayNghiViec = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiSinh = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NhanViens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NhanViens_DanhMucBuuCucs_BuuCucLamViecId",
                        column: x => x.BuuCucLamViecId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NhanViens_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VanDons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayGuiHang = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NoiDungHangHoa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GiaTriHangHoa = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrongLuong = table.Column<double>(type: "float", nullable: false),
                    COD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HoTenNguoiGui = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DienThoaiNguoiGui = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChiNguoiGui = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HoTenNguoiNhan = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    DienThoaiNguoiNhan = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DiaChiNguoiNhan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NhanVienLayHangId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NhanVienId = table.Column<int>(type: "int", nullable: true),
                    BuuCucHangDenId = table.Column<int>(type: "int", nullable: true),
                    CongTyGuiHangId = table.Column<int>(type: "int", nullable: true),
                    DichVuId = table.Column<int>(type: "int", nullable: true),
                    PhuongThucThanhToanId = table.Column<int>(type: "int", nullable: true),
                    HuyenId = table.Column<int>(type: "int", nullable: true),
                    KhachHangId = table.Column<int>(type: "int", nullable: true),
                    KhuVucId = table.Column<int>(type: "int", nullable: true),
                    TinhId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VanDons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VanDons_AppUsers_NhanVienLayHangId",
                        column: x => x.NhanVienLayHangId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_DanhMucBuuCucs_BuuCucHangDenId",
                        column: x => x.BuuCucHangDenId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_DanhMucCongTyGuiHangs_CongTyGuiHangId",
                        column: x => x.CongTyGuiHangId,
                        principalTable: "DanhMucCongTyGuiHangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_DanhMucDichVus_DichVuId",
                        column: x => x.DichVuId,
                        principalTable: "DanhMucDichVus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_DanhMucHuyens_HuyenId",
                        column: x => x.HuyenId,
                        principalTable: "DanhMucHuyens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_DanhMucKhuVucs_KhuVucId",
                        column: x => x.KhuVucId,
                        principalTable: "DanhMucKhuVucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_DanhMucPhuongThucThanhToans_PhuongThucThanhToanId",
                        column: x => x.PhuongThucThanhToanId,
                        principalTable: "DanhMucPhuongThucThanhToans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_DanhMucTinhs_TinhId",
                        column: x => x.TinhId,
                        principalTable: "DanhMucTinhs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_KhachHangs_KhachHangId",
                        column: x => x.KhachHangId,
                        principalTable: "KhachHangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VanDons_NhanViens_NhanVienId",
                        column: x => x.NhanVienId,
                        principalTable: "NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DangKyKienVanDes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    PhanLoaiHangBatThuongId = table.Column<int>(type: "int", nullable: true),
                    MieuTaVanDe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuaTrinhXuLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyKienVanDes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DangKyKienVanDes_DanhMucPhanLoaiHangBatThuongs_PhanLoaiHangBatThuongId",
                        column: x => x.PhanLoaiHangBatThuongId,
                        principalTable: "DanhMucPhanLoaiHangBatThuongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DangKyKienVanDes_VanDons_VanDonId",
                        column: x => x.VanDonId,
                        principalTable: "VanDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DuyetChuyenHoans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiKyNhanChuyenHoan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDaDuyet = table.Column<bool>(type: "bit", nullable: false),
                    NgayDuyetChuyenHoan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    BuuCucDuyetHangId = table.Column<int>(type: "int", nullable: true),
                    DangKyChuyenHoanId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuyetChuyenHoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuyetChuyenHoans_DangKyChuyenHoans_DangKyChuyenHoanId",
                        column: x => x.DangKyChuyenHoanId,
                        principalTable: "DangKyChuyenHoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DuyetChuyenHoans_DanhMucBuuCucs_BuuCucDuyetHangId",
                        column: x => x.BuuCucDuyetHangId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DuyetChuyenHoans_VanDons_VanDonId",
                        column: x => x.VanDonId,
                        principalTable: "VanDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuBaoHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VanDonId = table.Column<int>(type: "int", nullable: false),
                    BaoHangId = table.Column<int>(type: "int", nullable: true),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsTrongBao = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuBaoHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuBaoHangs_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuBaoHangs_BaoHangs_BaoHangId",
                        column: x => x.BaoHangId,
                        principalTable: "BaoHangs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuBaoHangs_VanDons_VanDonId",
                        column: x => x.VanDonId,
                        principalTable: "VanDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuChuyenHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuuCucId = table.Column<int>(type: "int", nullable: true),
                    SealXe = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    MaSealBao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsXeDi = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuChuyenHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuChuyenHangs_DanhMucBuuCucs_BuuCucId",
                        column: x => x.BuuCucId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuChuyenHangs_VanDons_VanDonId",
                        column: x => x.VanDonId,
                        principalTable: "VanDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuHangDens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    BuuCucId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuHangDens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuHangDens_DanhMucBuuCucs_BuuCucId",
                        column: x => x.BuuCucId,
                        principalTable: "DanhMucBuuCucs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuHangDens_VanDons_VanDonId",
                        column: x => x.VanDonId,
                        principalTable: "VanDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LichSuPhatHangs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    NhanVienId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LichSuPhatHangs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LichSuPhatHangs_NhanViens_NhanVienId",
                        column: x => x.NhanVienId,
                        principalTable: "NhanViens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LichSuPhatHangs_VanDons_VanDonId",
                        column: x => x.VanDonId,
                        principalTable: "VanDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhanLoaiVanDons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaVanDon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdDMPhanLoaiHangBT = table.Column<int>(type: "int", nullable: false),
                    VanDonId = table.Column<int>(type: "int", nullable: true),
                    DanhMucPhanLoaiHangBTId = table.Column<int>(type: "int", nullable: true),
                    AppUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanLoaiVanDons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhanLoaiVanDons_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanLoaiVanDons_DanhMucPhanLoaiHangBatThuongs_DanhMucPhanLoaiHangBTId",
                        column: x => x.DanhMucPhanLoaiHangBTId,
                        principalTable: "DanhMucPhanLoaiHangBatThuongs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanLoaiVanDons_VanDons_VanDonId",
                        column: x => x.VanDonId,
                        principalTable: "VanDons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AppConfigs",
                columns: new[] { "Key", "Value" },
                values: new object[,]
                {
                    { "HomeTitle", "This is home page of GiaoHangNhanh" },
                    { "HomeKeyword", "This is keyword of GiaoHangNhanh" },
                    { "HomeDescription", "This is Description of GiaoHangNhanh" }
                });

            migrationBuilder.InsertData(
                table: "AppRoles",
                columns: new[] { "Id", "ChucDanh", "ConcurrencyStamp", "CreatedUserId", "Description", "IsDelete", "ModifiedUserId", "Name", "NgaySua", "NgayTao", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), "Quản lý bưu cục", "998efde9-64fc-462d-a4a3-5b35a831ea68", new Guid("00000000-0000-0000-0000-000000000000"), "Quản lý bưu cục", false, new Guid("00000000-0000-0000-0000-000000000000"), "PostOfficeManager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PostOfficeManagerManager" },
                    { new Guid("8d04dce2-969a-435d-bba5-df3f325983dc"), "Quản lý", "9fdbaf23-f8e3-4a1a-9e3e-d2235dc6a623", new Guid("00000000-0000-0000-0000-000000000000"), "Quản lý", false, new Guid("00000000-0000-0000-0000-000000000000"), "Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "manager" },
                    { new Guid("8d04dce2-969a-435d-bba6-df3f325983dc"), "Nhân viên kho", "f98af6e7-a008-45ff-b3f4-f076bd9c5847", new Guid("00000000-0000-0000-0000-000000000000"), "Nhân viên kho", false, new Guid("00000000-0000-0000-0000-000000000000"), "WarehouseStaff", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "WarehouseStaff" },
                    { new Guid("8d04dce2-969a-435d-bba7-df3f325983dc"), "Nhân viên tiếp nhận khách hàng", "1a196ac0-d8d6-43b4-b3de-ee8e2647f2f0", new Guid("00000000-0000-0000-0000-000000000000"), "Nhân viên tiếp nhận khách hàng", false, new Guid("00000000-0000-0000-0000-000000000000"), "CustomerReceptionStaff", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CustomerReceptionStaff" },
                    { new Guid("8d04dce2-969a-435d-bba8-df3f325983dc"), "Nhân viên giao hàng", "ab9b41b0-048d-4000-b505-970edf334802", new Guid("00000000-0000-0000-0000-000000000000"), "Nhân viên giao hàng", false, new Guid("00000000-0000-0000-0000-000000000000"), "Shipper", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "shipper" },
                    { new Guid("8d04dce2-969a-435d-bba9-df3f325983dc"), "Quản lý chi nhánh", "cecf6288-681e-46ac-9edf-f9208979675c", new Guid("00000000-0000-0000-0000-000000000000"), "Quản lý chi nhánh", false, new Guid("00000000-0000-0000-0000-000000000000"), "BranchManager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "BranchManager" },
                    { new Guid("8d04dce2-969a-435d-bba3-df3f325983dc"), "Quản trị hệ thống", "040e40a8-6aff-4991-97ab-4f971f6680bb", new Guid("00000000-0000-0000-0000-000000000000"), "Quản trị hệ thống", false, new Guid("00000000-0000-0000-0000-000000000000"), "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" },
                    { new Guid("38a2e063-1b9f-48d0-b7f8-55f2e835e8f0"), null, "379fb1c1-c3e6-4eee-a409-a87d4757482d", new Guid("00000000-0000-0000-0000-000000000000"), "Administrator role", false, new Guid("00000000-0000-0000-0000-000000000000"), "admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin" }
                });

            migrationBuilder.InsertData(
                table: "AppUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("38a2e063-1b9f-48d0-b7f8-55f2e835e8f0"), new Guid("447fe343-9985-412c-bb19-c6f398bc014f") });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "AccessFailedCount", "ActivateDate", "Address", "Avatar", "Code", "ConcurrencyStamp", "CreatedDate", "CreatedUserId", "Description", "Dob", "Email", "EmailConfirmed", "FirstName", "IsActive", "IsDelete", "LastName", "LeaveDate", "LockoutEnabled", "LockoutEnd", "MaNhanVien", "ModifiedDate", "ModifiedUserId", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "StartingDate", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), 0, null, null, null, null, "40a1f195-914b-4443-ab7d-0d605109a0d4", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), null, new DateTime(2022, 10, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@gmail.com", true, "Tram", true, false, "Nguyen", null, false, null, "NV64202344", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("00000000-0000-0000-0000-000000000000"), "admin@gmail.com", "admin", "AQAAAAEAACcQAAAAEPc0MlY7KfgF/aWCVfBLjS7OI4v+djUnb+DkbnSO+m4XShKw5uZFAqjPOL1HsTySJQ==", null, false, "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "admin" });

            migrationBuilder.InsertData(
                table: "DangKyChuyenHoans",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDangKyChuyenHoan", "IsDefault", "IsDeleted", "MieuTaNguyenNhan", "ModifiedDate", "ModifiedUserId", "Name", "NguyenNhan", "SortOrder", "VanDonId" },
                values: new object[] { 1, null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(4197), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, true, false, false, "Khách gọi không nghe máy", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(4200), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, "Khách không nghe máy", 0, 1 });

            migrationBuilder.InsertData(
                table: "DanhMucDichVus",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder" },
                values: new object[] { 1, "GHQT", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(3940), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(3943), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Giao hàng quốc tế", 0 });

            migrationBuilder.InsertData(
                table: "DanhMucPhanLoaiHangBatThuongs",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder" },
                values: new object[] { 1, "KHTC", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(9222), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(9224), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Khách hàng từ chối nhận", 0 });

            migrationBuilder.InsertData(
                table: "DanhMucPhuongThucThanhToans",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder" },
                values: new object[] { 1, "TT", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(6034), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(6037), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Thanh toán trực tiếp", 0 });

            migrationBuilder.InsertData(
                table: "DanhMucTinhs",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder" },
                values: new object[] { 1, "KH", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(3165), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(3167), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Khánh Hòa", 0 });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder" },
                values: new object[,]
                {
                    { 1, "N", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(6283), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(6285), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Nam", 0 },
                    { 2, "NU", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(6293), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(6294), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Nữ", 0 }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "ActionName", "Code", "ControllerName", "CreatedDate", "CreatedUserId", "Description", "Icon", "IsActive", "IsDefault", "IsDeleted", "Link", "ModifiedDate", "ModifiedUserId", "Name", "ParentId", "SortOrder" },
                values: new object[,]
                {
                    { 27, "Index", "DM_GT", "Gender", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2733), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/Gender/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2733), new Guid("00000000-0000-0000-0000-000000000000"), "Giới tính", 14, 6 },
                    { 22, "Index", "QTHT_DMHT_T", "Tinh", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2735), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/Tinh/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2736), new Guid("00000000-0000-0000-0000-000000000000"), "Tỉnh", 14, 4 },
                    { 23, "Index", "QTHT_DMHT_H", "Huyen", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2738), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/Huyen/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2739), new Guid("00000000-0000-0000-0000-000000000000"), "Huyện", 14, 5 },
                    { 24, "Index", "QTHT_DMHT_KV", "KhuVuc", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2741), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/KhuVuc/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2742), new Guid("00000000-0000-0000-0000-000000000000"), "Khu vực", 14, 6 },
                    { 25, "Index", "QTHT_DMHT_BC", "BuuCuc", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2743), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/BuuCuc/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2744), new Guid("00000000-0000-0000-0000-000000000000"), "Bưu cục", 14, 7 },
                    { 21, "Index", "QTHT_PQ", "AppRole", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2755), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/AppRole/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2756), new Guid("00000000-0000-0000-0000-000000000000"), "Phân quyền", 18, 3 },
                    { 19, "Index", "QTHT_AC", "Staff", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2749), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/Staff/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2750), new Guid("00000000-0000-0000-0000-000000000000"), "Người sử dụng", 18, 1 },
                    { 20, "Index", "QTHT_NV", "NhanVien", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2752), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/NhanVien/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2753), new Guid("00000000-0000-0000-0000-000000000000"), "Nhân Viên", 18, 2 },
                    { 26, "Index", "QTHT_DMHT_M", "Menu", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2758), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/Menu/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2758), new Guid("00000000-0000-0000-0000-000000000000"), "Menu", 18, 8 },
                    { 30, null, "CH", null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2760), new Guid("00000000-0000-0000-0000-000000000000"), null, "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>", true, false, false, "", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2761), new Guid("00000000-0000-0000-0000-000000000000"), "Chuyển hoàn", null, 4 },
                    { 32, "Index", "CH_DKCH", "DangKyChuyenHoan", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2764), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/DangKyChuyenHoan/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2765), new Guid("00000000-0000-0000-0000-000000000000"), "Đăng ký chuyển hoàn", 30, 2 },
                    { 33, "Index", "CH_DCH", "DuyetChuyenHoan", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2775), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/DuyetChuyenHoan/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2776), new Guid("00000000-0000-0000-0000-000000000000"), "Duyệt chuyển hoàn", 30, 3 },
                    { 18, null, "QTHT", null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2746), new Guid("00000000-0000-0000-0000-000000000000"), null, "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24' fill='none'><path opacity='0.25' d='M2 6.5C2 4.01472 4.01472 2 6.5 2H17.5C19.9853 2 22 4.01472 22 6.5V6.5C22 8.98528 19.9853 11 17.5 11H6.5C4.01472 11 2 8.98528 2 6.5V6.5Z' fill='#12131A'></path><path d='M20 6.5C20 7.88071 18.8807 9 17.5 9C16.1193 9 15 7.88071 15 6.5C15 5.11929 16.1193 4 17.5 4C18.8807 4 20 5.11929 20 6.5Z' fill='#12131A'></path><path opacity='0.25' d='M2 17.5C2 15.0147 4.01472 13 6.5 13H17.5C19.9853 13 22 15.0147 22 17.5V17.5C22 19.9853 19.9853 22 17.5 22H6.5C4.01472 22 2 19.9853 2 17.5V17.5Z' fill='#12131A'></path><path d='M9 17.5C9 18.8807 7.88071 20 6.5 20C5.11929 20 4 18.8807 4 17.5C4 16.1193 5.11929 15 6.5 15C7.88071 15 9 16.1193 9 17.5Z' fill='#12131A'></path></svg></span></span>", true, false, false, "", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2747), new Guid("00000000-0000-0000-0000-000000000000"), "Quản trị hệ thống", null, 5 },
                    { 7, "Index", "DM_PLHBT", "PhanLoaiHangBatThuong", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2730), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/PhanLoaiHangBatThuong/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2731), new Guid("00000000-0000-0000-0000-000000000000"), "Phân loại hàng bất thường", 14, 5 },
                    { 15, "Index", "DM_DV", "DanhMucDichVu", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2721), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/DichVu/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2722), new Guid("00000000-0000-0000-0000-000000000000"), "Dịch vụ", 14, 1 },
                    { 16, "Index", "DM_CTGH", "DanhMucCongTyGuiHang", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2723), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/CongTyGuiHang/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2724), new Guid("00000000-0000-0000-0000-000000000000"), "Công ty gửi hàng", 14, 2 },
                    { 1, null, "KH", null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(138), new Guid("00000000-0000-0000-0000-000000000000"), null, "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns = 'http://www.w3.org/2000/svg' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><path d = 'M18,14 C16.3431458,14 15,12.6568542 15,11 C15,9.34314575 16.3431458,8 18,8 C19.6568542,8 21,9.34314575 21,11 C21,12.6568542 19.6568542,14 18,14 Z M9,11 C6.790861,11 5,9.209139 5,7 C5,4.790861 6.790861,3 9,3 C11.209139,3 13,4.790861 13,7 C13,9.209139 11.209139,11 9,11 Z' fill='#000000' fill-rule='nonzero' opacity='0.3'></path><path d = 'M17.6011961,15.0006174 C21.0077043,15.0378534 23.7891749,16.7601418 23.9984937,20.4 C24.0069246,20.5466056 23.9984937,21 23.4559499,21 L19.6,21 C19.6,18.7490654 18.8562935,16.6718327 17.6011961,15.0006174 Z M0.00065168429,20.1992055 C0.388258525,15.4265159 4.26191235,13 8.98334134,13 C13.7712164,13 17.7048837,15.2931929 17.9979143,20.2 C18.0095879,20.3954741 17.9979143,21 17.2466999,21 C13.541124,21 8.03472472,21 0.727502227,21 C0.476712155,21 -0.0204617505,20.45918 0.00065168429,20.1992055 Z' fill='#000000' fill-rule='nonzero'></path></svg></span></span>", true, false, false, "", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(408), new Guid("00000000-0000-0000-0000-000000000000"), "Khách hàng", null, 1 },
                    { 2, "Index", "KH_KH", "KhachHang", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2262), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/KhachHang/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2265), new Guid("00000000-0000-0000-0000-000000000000"), "Khách hàng", 1, 1 },
                    { 3, null, "VD", null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2672), new Guid("00000000-0000-0000-0000-000000000000"), null, "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>", true, false, false, "", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2674), new Guid("00000000-0000-0000-0000-000000000000"), "Vận đơn", null, 2 },
                    { 4, "Index", "VD_TVD", "VanDon", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2677), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/VanDon/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2678), new Guid("00000000-0000-0000-0000-000000000000"), "Tra vận đơn", 3, 1 },
                    { 5, null, "TT", null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2681), new Guid("00000000-0000-0000-0000-000000000000"), null, "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>", true, false, false, "", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2682), new Guid("00000000-0000-0000-0000-000000000000"), "Thao tác", null, 3 },
                    { 6, "Index", "TT_KN", "KyNhan", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2685), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/KyNhan/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2685), new Guid("00000000-0000-0000-0000-000000000000"), "Ký nhận", 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "ActionName", "Code", "ControllerName", "CreatedDate", "CreatedUserId", "Description", "Icon", "IsActive", "IsDefault", "IsDeleted", "Link", "ModifiedDate", "ModifiedUserId", "Name", "ParentId", "SortOrder" },
                values: new object[,]
                {
                    { 8, "Index", "TT_LSNK", "LichSuNhapKho", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2689), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/LichSuNhapKho/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2690), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử nhập kho", 5, 3 },
                    { 9, "Index", "TT_LSDB", "DongBaoHang", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2694), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/DongBaoHang/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2695), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử đóng bao", 5, 4 },
                    { 10, "Index", "TT_LSTB", "ThaoBaoHang", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2696), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/ThaoBaoHang/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2697), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử tháo bao", 5, 5 },
                    { 11, "Index", "TT_LSXDE", "LichSuXeDen", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2699), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/LichSuXeDen/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2700), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử xe đến", 5, 6 },
                    { 13, "Index", "TT_LSGH", "LichSuGuiHang", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2707), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/LichSuGuiHang/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2708), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử gửi hàng", 5, 8 },
                    { 34, "Index", "TT_LSHD", "LichSuHangDen", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2709), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/LichSuHangDen/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2710), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử hàng đến", 5, 9 },
                    { 29, "Index", "TT_LSPH", "LichSuPhatHang", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2713), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/LichSuPhatHang/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2713), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử phát hàng", 5, 2 },
                    { 31, "Index", "TT_DKKVD", "DangKyKienVanDe", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2715), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/DangKyKienVanDe/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2716), new Guid("00000000-0000-0000-0000-000000000000"), "Đăng ký kiện vấn đề", 5, 10 },
                    { 14, null, "DM", null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2718), new Guid("00000000-0000-0000-0000-000000000000"), null, "<span class='menu-icon'><span class='svg-icon svg-icon-2'><svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' width='24px' height='24px' viewBox='0 0 24 24' version='1.1'><g stroke='none' stroke-width='1' fill='none' fill-rule='evenodd'><rect x='0' y='0' width='24' height='24'></rect><rect fill='#000000' x='4' y='4' width='7' height='7' rx='1.5'></rect><path d='M5.5,13 L9.5,13 C10.3284271,13 11,13.6715729 11,14.5 L11,18.5 C11,19.3284271 10.3284271,20 9.5,20 L5.5,20 C4.67157288,20 4,19.3284271 4,18.5 L4,14.5 C4,13.6715729 4.67157288,13 5.5,13 Z M14.5,4 L18.5,4 C19.3284271,4 20,4.67157288 20,5.5 L20,9.5 C20,10.3284271 19.3284271,11 18.5,11 L14.5,11 C13.6715729,11 13,10.3284271 13,9.5 L13,5.5 C13,4.67157288 13.6715729,4 14.5,4 Z M14.5,13 L18.5,13 C19.3284271,13 20,13.6715729 20,14.5 L20,18.5 C20,19.3284271 19.3284271,20 18.5,20 L14.5,20 C13.6715729,20 13,19.3284271 13,18.5 L13,14.5 C13,13.6715729 13.6715729,13 14.5,13 Z' fill='#000000' opacity='0.3'></path></g></svg></span></span>", true, false, false, "", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2719), new Guid("00000000-0000-0000-0000-000000000000"), "Danh mục", null, 4 },
                    { 17, "Index", "DM_PTTT", "DanhMucPhuongThucThanhToan", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2727), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/PhuongThucThanhToan/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2728), new Guid("00000000-0000-0000-0000-000000000000"), "Phương thức thanh toán", 14, 3 },
                    { 12, "Index", "TT_LSXD", "LichSuXeDi", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2702), new Guid("00000000-0000-0000-0000-000000000000"), null, null, true, false, false, "/LichSuXeDi/Index", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(2704), new Guid("00000000-0000-0000-0000-000000000000"), "Lịch sử xe đi", 5, 7 }
                });

            migrationBuilder.InsertData(
                table: "DanhMucHuyens",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder", "TinhId" },
                values: new object[] { 1, "NT", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(9480), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(9482), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Thành phố Nha Trang", 0, 1 });

            migrationBuilder.InsertData(
                table: "DanhMucKhuVucs",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "HuyenId", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder" },
                values: new object[] { 1, "VN", new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(63), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, 1, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(68), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Vĩnh Ngọc", 0 });

            migrationBuilder.InsertData(
                table: "DanhMucBuuCucs",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "HuyenId", "IsActive", "IsDefault", "IsDeleted", "KhuVucId", "ModifiedDate", "ModifiedUserId", "Name", "SortOrder", "TinhId" },
                values: new object[] { 1, "AL1", new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(605), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, 1, false, false, false, 1, new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(608), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Alpha1", 0, 1 });

            migrationBuilder.InsertData(
                table: "DanhMucCongTyGuiHangs",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "DiaChi", "HuyenId", "IsActive", "IsDefault", "IsDeleted", "KhuVucId", "ModifiedDate", "ModifiedUserId", "Name", "PhoneNumber", "SortOrder", "TinhId" },
                values: new object[] { 1, "SPC", new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(1450), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, null, 1, false, false, false, 1, new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(1453), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), "Shopee", "0958624542", 0, 1 });

            migrationBuilder.InsertData(
                table: "NhanViens",
                columns: new[] { "Id", "BuuCucLamViecId", "CCCD", "Code", "CreatedDate", "CreatedUserId", "Description", "DiaChi", "Email", "FirstName", "GenderId", "IsActive", "IsDefault", "IsDeleted", "LastName", "ModifiedDate", "ModifiedUserId", "Name", "NgayLamViec", "NgayNghiViec", "NgaySinh", "NoiSinh", "SoDienThoai", "SortOrder" },
                values: new object[] { 1, 1, "123456789", "NV01", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(7084), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, "79 Mai Thi Dong, Khanh Hoa", "vuthanh@gmail.com", "Thanh", 1, true, false, false, "Vu", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(6581), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, null, null, null, "Ninh Thuan", "0123456789", 0 });

            migrationBuilder.InsertData(
                table: "NhanViens",
                columns: new[] { "Id", "BuuCucLamViecId", "CCCD", "Code", "CreatedDate", "CreatedUserId", "Description", "DiaChi", "Email", "FirstName", "GenderId", "IsActive", "IsDefault", "IsDeleted", "LastName", "ModifiedDate", "ModifiedUserId", "Name", "NgayLamViec", "NgayNghiViec", "NgaySinh", "NoiSinh", "SoDienThoai", "SortOrder" },
                values: new object[] { 2, 1, "102030405060", "NV02", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(8251), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, "79 Mai Thi Dong, Khanh Hoa", "nguyentram@gmail.com", "Tram", 2, true, false, false, "Nguyen", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(8249), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, null, null, null, "Ninh Thuan", "0102030405", 0 });

            migrationBuilder.InsertData(
                table: "VanDons",
                columns: new[] { "Id", "BuuCucHangDenId", "COD", "Code", "CongTyGuiHangId", "CreatedDate", "CreatedUserId", "Description", "DiaChiNguoiGui", "DiaChiNguoiNhan", "DichVuId", "DienThoaiNguoiGui", "DienThoaiNguoiNhan", "GiaTriHangHoa", "HoTenNguoiGui", "HoTenNguoiNhan", "HuyenId", "IsActive", "IsDefault", "IsDeleted", "KhachHangId", "KhuVucId", "ModifiedDate", "ModifiedUserId", "Name", "NgayGuiHang", "NhanVienId", "NhanVienLayHangId", "NoiDungHangHoa", "PhuongThucThanhToanId", "SortOrder", "TinhId", "TrongLuong" },
                values: new object[] { 1, 1, 12000m, "920293392206", 1, new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(2498), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, "123 Thống nhất, Ninh Thuận", "79 Mai Thi Dong, Khánh Hòa", 1, "01627038692", "0905684251", 0m, "Shopee", "Vũ Thành", null, false, false, false, null, null, new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(2500), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(4856), 1, null, "Quần áo", 1, 0, null, 0.59999999999999998 });

            migrationBuilder.InsertData(
                table: "DangKyKienVanDes",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "MieuTaVanDe", "ModifiedDate", "ModifiedUserId", "Name", "PhanLoaiHangBatThuongId", "QuaTrinhXuLy", "SortOrder", "VanDonId" },
                values: new object[] { 1, "DKKVD", new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(5520), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, "Đơn hàng bị vỡ", new DateTime(2023, 4, 6, 14, 20, 44, 406, DateTimeKind.Local).AddTicks(5524), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, 1, "Liên hệ người gửi", 0, 1 });

            migrationBuilder.InsertData(
                table: "DuyetChuyenHoans",
                columns: new[] { "Id", "BuuCucDuyetHangId", "Code", "CreatedDate", "CreatedUserId", "DangKyChuyenHoanId", "Description", "IsActive", "IsDaDuyet", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "NgayDuyetChuyenHoan", "NguoiKyNhanChuyenHoan", "SortOrder", "VanDonId" },
                values: new object[] { 1, null, null, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(5200), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), 1, null, false, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(5203), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tram", 0, 1 });

            migrationBuilder.InsertData(
                table: "LichSuPhatHangs",
                columns: new[] { "Id", "Code", "CreatedDate", "CreatedUserId", "Description", "IsActive", "IsDefault", "IsDeleted", "ModifiedDate", "ModifiedUserId", "Name", "NhanVienId", "SortOrder", "VanDonId" },
                values: new object[] { 1, "PH", new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(8942), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, false, false, false, new DateTime(2023, 4, 6, 14, 20, 44, 405, DateTimeKind.Local).AddTicks(8555), new Guid("447fe343-9985-412c-bb19-c6f398bc014f"), null, 1, 0, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKienVanDes_PhanLoaiHangBatThuongId",
                table: "DangKyKienVanDes",
                column: "PhanLoaiHangBatThuongId");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyKienVanDes_VanDonId",
                table: "DangKyKienVanDes",
                column: "VanDonId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucBuuCucs_HuyenId",
                table: "DanhMucBuuCucs",
                column: "HuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucBuuCucs_KhuVucId",
                table: "DanhMucBuuCucs",
                column: "KhuVucId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucBuuCucs_TinhId",
                table: "DanhMucBuuCucs",
                column: "TinhId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucCongTyGuiHangs_HuyenId",
                table: "DanhMucCongTyGuiHangs",
                column: "HuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucCongTyGuiHangs_KhuVucId",
                table: "DanhMucCongTyGuiHangs",
                column: "KhuVucId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucCongTyGuiHangs_TinhId",
                table: "DanhMucCongTyGuiHangs",
                column: "TinhId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucHuyens_TinhId",
                table: "DanhMucHuyens",
                column: "TinhId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucKhuVucs_HuyenId",
                table: "DanhMucKhuVucs",
                column: "HuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_DuyetChuyenHoans_BuuCucDuyetHangId",
                table: "DuyetChuyenHoans",
                column: "BuuCucDuyetHangId");

            migrationBuilder.CreateIndex(
                name: "IX_DuyetChuyenHoans_DangKyChuyenHoanId",
                table: "DuyetChuyenHoans",
                column: "DangKyChuyenHoanId");

            migrationBuilder.CreateIndex(
                name: "IX_DuyetChuyenHoans_VanDonId",
                table: "DuyetChuyenHoans",
                column: "VanDonId");

            migrationBuilder.CreateIndex(
                name: "IX_KyNhans_AppUserId",
                table: "KyNhans",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_KyNhans_BuuCucId",
                table: "KyNhans",
                column: "BuuCucId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuBaoHangs_AppUserId",
                table: "LichSuBaoHangs",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuBaoHangs_BaoHangId",
                table: "LichSuBaoHangs",
                column: "BaoHangId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuBaoHangs_VanDonId",
                table: "LichSuBaoHangs",
                column: "VanDonId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuChuyenHangs_BuuCucId",
                table: "LichSuChuyenHangs",
                column: "BuuCucId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuChuyenHangs_VanDonId",
                table: "LichSuChuyenHangs",
                column: "VanDonId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuGuiHangs_BuuCucId",
                table: "LichSuGuiHangs",
                column: "BuuCucId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuHangDens_BuuCucId",
                table: "LichSuHangDens",
                column: "BuuCucId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuHangDens_VanDonId",
                table: "LichSuHangDens",
                column: "VanDonId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuNhapKhos_AppUserId",
                table: "LichSuNhapKhos",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuNhapKhos_BuuCucId",
                table: "LichSuNhapKhos",
                column: "BuuCucId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuPhatHangs_NhanVienId",
                table: "LichSuPhatHangs",
                column: "NhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_LichSuPhatHangs_VanDonId",
                table: "LichSuPhatHangs",
                column: "VanDonId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuAppRoles_MenuId",
                table: "MenuAppRoles",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_BuuCucLamViecId",
                table: "NhanViens",
                column: "BuuCucLamViecId");

            migrationBuilder.CreateIndex(
                name: "IX_NhanViens_GenderId",
                table: "NhanViens",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_PhanLoaiVanDons_AppUserId",
                table: "PhanLoaiVanDons",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PhanLoaiVanDons_DanhMucPhanLoaiHangBTId",
                table: "PhanLoaiVanDons",
                column: "DanhMucPhanLoaiHangBTId");

            migrationBuilder.CreateIndex(
                name: "IX_PhanLoaiVanDons_VanDonId",
                table: "PhanLoaiVanDons",
                column: "VanDonId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_BuuCucHangDenId",
                table: "VanDons",
                column: "BuuCucHangDenId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_CongTyGuiHangId",
                table: "VanDons",
                column: "CongTyGuiHangId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_DichVuId",
                table: "VanDons",
                column: "DichVuId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_HuyenId",
                table: "VanDons",
                column: "HuyenId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_KhachHangId",
                table: "VanDons",
                column: "KhachHangId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_KhuVucId",
                table: "VanDons",
                column: "KhuVucId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_NhanVienId",
                table: "VanDons",
                column: "NhanVienId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_NhanVienLayHangId",
                table: "VanDons",
                column: "NhanVienLayHangId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_PhuongThucThanhToanId",
                table: "VanDons",
                column: "PhuongThucThanhToanId");

            migrationBuilder.CreateIndex(
                name: "IX_VanDons_TinhId",
                table: "VanDons",
                column: "TinhId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConfigs");

            migrationBuilder.DropTable(
                name: "AppRoleClaims");

            migrationBuilder.DropTable(
                name: "AppUserClaims");

            migrationBuilder.DropTable(
                name: "AppUserLogins");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserTokens");

            migrationBuilder.DropTable(
                name: "DangKyKienVanDes");

            migrationBuilder.DropTable(
                name: "DuyetChuyenHoans");

            migrationBuilder.DropTable(
                name: "KyNhans");

            migrationBuilder.DropTable(
                name: "LichSuBaoHangs");

            migrationBuilder.DropTable(
                name: "LichSuChuyenHangs");

            migrationBuilder.DropTable(
                name: "LichSuGuiHangs");

            migrationBuilder.DropTable(
                name: "LichSuHangDens");

            migrationBuilder.DropTable(
                name: "LichSuNhapKhos");

            migrationBuilder.DropTable(
                name: "LichSuPhatHangs");

            migrationBuilder.DropTable(
                name: "MenuAppRoles");

            migrationBuilder.DropTable(
                name: "PhanLoaiVanDons");

            migrationBuilder.DropTable(
                name: "DangKyChuyenHoans");

            migrationBuilder.DropTable(
                name: "BaoHangs");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "DanhMucPhanLoaiHangBatThuongs");

            migrationBuilder.DropTable(
                name: "VanDons");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "DanhMucCongTyGuiHangs");

            migrationBuilder.DropTable(
                name: "DanhMucDichVus");

            migrationBuilder.DropTable(
                name: "DanhMucPhuongThucThanhToans");

            migrationBuilder.DropTable(
                name: "KhachHangs");

            migrationBuilder.DropTable(
                name: "NhanViens");

            migrationBuilder.DropTable(
                name: "DanhMucBuuCucs");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "DanhMucKhuVucs");

            migrationBuilder.DropTable(
                name: "DanhMucHuyens");

            migrationBuilder.DropTable(
                name: "DanhMucTinhs");
        }
    }
}
