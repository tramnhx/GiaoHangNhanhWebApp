using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.UI.AdminApp
{
    public class AdminAppLeftSideBarDto
    {
        public string Logo { set; get; }
        public List<MenuDto> Menus { set; get; }
    }
}
