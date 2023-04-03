using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs
{
    public class LichSuBaoHangDto
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string IsDongBao { get; set; }
        public string IsMoBao { get; set; }
        public bool IsTrongBao { get; set; }
        public int BaoHangId { get; set; }
        public string CreatedDate { set; get; }
        public string MoBaoDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public IList<VanDonDto> VanDons { get; set; }
        public VanDonDto VanDon { get; set; }
        public UserDto AppUser { get; set; }
    }
}
