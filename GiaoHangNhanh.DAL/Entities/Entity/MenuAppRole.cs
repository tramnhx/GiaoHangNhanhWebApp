using GiaoHangNhanh.DAL.Entities.EntityDto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class MenuAppRole : BaseEntity
    {
        public Guid AppRoleId { set; get; }
        public AppRole AppRole { set; get; }
        public int MenuId { set; get; }
        public Menu Menu { set; get; }
        public bool IsAllow { set; get; }
        public MenuAppRoleType MenuAppRoleType { set; get; }
    }
}
