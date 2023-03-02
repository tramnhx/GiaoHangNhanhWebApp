using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Statuses.AppUserStatus
{
    public class AppUserStatusRequest
    {
        public int? Id { set; get; }
        public string GhiChu { set; get; }
        public string Name { set; get; }
        public int SortOrder { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
