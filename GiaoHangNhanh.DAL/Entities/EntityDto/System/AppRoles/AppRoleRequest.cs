using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles
{
    public class AppRoleRequest
    {
        public string Id { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string ChucDanh { get; set; }
        public string Description { set; get; }
        public int LanguageId { set; get; }
        public DateTime NgayTao { get; set; }
        public DateTime NgaySua { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }

    }
}
