using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus
{
    public class MenuDto : BaseDto
    {
        public int? ParentId { set; get; }
        public string Icon { set; get; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Link { set; get; }
    }
}