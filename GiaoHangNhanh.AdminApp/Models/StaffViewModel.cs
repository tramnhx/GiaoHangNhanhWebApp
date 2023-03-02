using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace GiaoHangNhanh.AdminApp.Models
{
    public class StaffViewModel : BaseViewModel
    {
        public UserDto Staff { set; get; }
        public List<SelectListItem> AppRoles { get; set; }
        public string FileNoImagePerson { set; get; }
    }
}
