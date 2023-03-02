
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles;
using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string MaNhanVien { get; set; }
        public int? BuuCucId { get; set; }
        public BuuCucDto BuuCuc { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? Dob { get; set; }
        public IList<AppRoleDto> AppRoles { get; set; }
        public string Avatar { set; get; }
        public string StrCreatedDate { set; get; }
        public List<MenuAppRoleDto> MenuAppRoles { set; get; }
        public bool IsAllowView { set; get; }
        public bool IsAllowEdit { set; get; }
        public bool IsAllowDelete { set; get; }
        public bool IsAllowDownloadExcel { set; get; }
        public bool IsActive { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime? LeaveDate { set; get; }
        public string StrStartingDate { set; get; }
        public string StrLeaveDate { set; get; }
        public string Address { set; get; }
        public string Id_Image_FullName_Email { set; get; }
    }
}