using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class Gender : BaseEntity
    {
        public ICollection<NhanVien> NhanViens { get; set; }
    }
}
