using GiaoHangNhanh.DAL.Entities.EntityDto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Models
{
    public class MenuAppRoleViewModel
    {
        public int Id { get; set; }
        public string AppRoleId { set; get; }
        public AppRoleViewModel AppRole { set; get; }
        public int MenuId { set; get; }
        public MenuViewModel Menu { set; get; }
        public bool IsAllow { set; get; }
        public MenuAppRoleType MenuAppRoleType { set; get; }
    }
}
