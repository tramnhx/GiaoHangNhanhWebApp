using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using System.Collections.Generic;

namespace GiaoHangNhanh.AdminApp.Models
{
    public class BaseViewModel
    {
        public UserDto CurrentUserRole { set; get; }
        public string PageTitle { set; get; }
        public List<string> Breadcrumbs { set; get; }
    }
}