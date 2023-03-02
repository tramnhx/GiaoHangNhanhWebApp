using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class DangKyChuyenHoan : BaseEntity
    {
        public int? VanDonId { get; set; }
        public string NguyenNhan { get; set; }
        public string MieuTaNguyenNhan { get; set; }
    }
}
