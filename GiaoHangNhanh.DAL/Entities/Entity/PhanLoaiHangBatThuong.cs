using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class PhanLoaiHangBatThuong : BaseEntity
    {
        public ICollection<DangKyKienVanDe> DangKyKienVanDes { get; set; }
    }
}
