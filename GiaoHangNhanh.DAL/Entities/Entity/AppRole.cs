using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class AppRole : IdentityRole<Guid>
    {
        public string ChucDanh { set; get; }
        public string Description { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgaySua { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public bool IsDelete { get; set; }
        public ICollection<MenuAppRole> MenuAppRoles { get; set; }
    }
}
