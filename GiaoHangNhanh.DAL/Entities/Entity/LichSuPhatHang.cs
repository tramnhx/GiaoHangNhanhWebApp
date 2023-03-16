using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class LichSuPhatHang : BaseEntity
    {
        public int? VanDonId { get; set; }
        public VanDon VanDon { get; set; }
        public int? NhanVienId { get; set; }
        public NhanVien NhanVien { get; set; }
    }
}
