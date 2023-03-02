using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Users
{
    public class UserRequest
    {
        public string Id { set; get; }
        public string CreatedUserId { set; get; }
        public string ModifiedUserId { set; get; }
        public string Code { set; get; }
        public int AppUserStatusId { set; get; }
        public string Address { set; get; }
        public string HoTen { get; set; }
        public string MaNhanVien { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Dob { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? GenderId { get; set; }
        public List<string> AppRoleCodes { set; get; }
        public bool IsActive { set; get; }
        public IFormFile Avatar { set; get; }
        public int LanguageId { set; get; }
        public string Description { set; get; }
        public string LeaveDate { set; get; }
        public string StartingDate { set; get; }

    }
}
