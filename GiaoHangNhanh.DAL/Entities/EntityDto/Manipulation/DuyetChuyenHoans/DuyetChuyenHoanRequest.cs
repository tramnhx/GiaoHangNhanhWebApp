using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DuyetChuyenHoans
{
    public class DuyetChuyenHoanRequest
    {
        public int? Id { get; set; }
        public string NguoiKyNhanChuyenHoan { get; set; }
        public bool IsDaDuyet { get; set; }
        public DateTime? NgayDuyetChuyenHoan { get; set; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }

        public int? VanDonId { get; set; }
        public int? BuuCucDuyetChuyenHoanId { get; set; }
        public int? DangKyChuyenHoanId { get; set; }
    }
}
