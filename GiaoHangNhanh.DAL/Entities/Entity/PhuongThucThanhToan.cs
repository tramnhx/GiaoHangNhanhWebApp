using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class PhuongThucThanhToan : BaseEntity
    {
        public ICollection<VanDon> VanDons { get; set; }
    }
}
