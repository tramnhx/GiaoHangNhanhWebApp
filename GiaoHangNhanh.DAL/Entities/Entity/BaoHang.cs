using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class BaoHang : BaseEntity
    {
        public bool IsDongBao { get; set; }
        public DateTime? MoBaoDate { get; set; }
        public ICollection<LichSuBaoHang> LichSuBaoHangs { get; set; }
    }
}
