using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans
{
    public class DangKyChuyenHoanRequest
    {
        public int? Id { get; set; }
        public int VanDonId { get; set; }
        public string MieuTaNguyenNhan { get; set; }
        public string NguyenNhan { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
    }
}
