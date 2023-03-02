using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class Menu : BaseEntity
    {
        public int? ParentId { set; get; }
        public string Icon { set; get; }
        public string Link { set; get; }
        public string ControllerName { set; get; }
        public string ActionName { set; get; }
        public ICollection<MenuAppRole> MenuAppRoles { get; set; }
    }
}
