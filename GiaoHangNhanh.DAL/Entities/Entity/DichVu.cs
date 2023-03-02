using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class DichVu : BaseEntity
    {
        public ICollection<VanDon> VanDons { get; set; }
    }
}
