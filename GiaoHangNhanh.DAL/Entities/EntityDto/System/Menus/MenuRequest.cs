using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus
{
    public class MenuRequest
    {
        public int? Id { set; get; }
        public int? ParentId { set; get; }
        public string Icon { set; get; }
        public string Link { set; get; }
        public int SortOrder { set; get; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string Description { set; get; }
        public int LanguageId { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}