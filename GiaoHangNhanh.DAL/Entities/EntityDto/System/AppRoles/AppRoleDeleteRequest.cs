using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles
{
    public class AppRoleDeleteRequest
    {
        public List<string> Ids { set; get; }
        public Guid UserDelete { get; set; }
    }
}
