using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class DangKyKienVanDe : BaseEntity
    {
        public int? VanDonId { get; set; }
        public int? PhanLoaiHangBatThuongId { get; set; }

        public VanDon VanDon { get; set; }
        public PhanLoaiHangBatThuong PhanLoaiHangBatThuong { get; set; }

    }
}
