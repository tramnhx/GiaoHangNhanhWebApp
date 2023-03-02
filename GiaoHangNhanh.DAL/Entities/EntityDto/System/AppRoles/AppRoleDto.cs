using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles
{
    public class AppRoleDto
    {
        public string Id { set; get; }
        public string Code { set; get; }
        public string ChucDanh { get; set; }
        public string Name { set; get; }
        public string Description { set; get; }
        public bool IsDelete { get; set; }
        public Guid UserId { set; get; }
        public int LanguageId { set; get; }
        public DateTime NgayTao { get; set; }
        public DateTime NgaySua { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}