using System;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            IsDeleted = false;
            IsDefault = false;
            SortOrder = 0;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Description { get; set; }
        public int SortOrder { set; get; }
        public bool IsDeleted { set; get; }
        public bool IsDefault { set; get; }
        public bool IsActive { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }

    }
}
