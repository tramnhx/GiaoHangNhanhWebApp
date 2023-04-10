using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuHangDens
{
    public class LichSuHangDenDto : BaseDto
    {
        public string StrCreatedDate { get; set; }
        public BuuCucDto BuuCuc { get; set; }
        public VanDonDto VanDon { get; set; }
        public IList<VanDonDto> VanDons { get; set; }
        public IList<string> MaVanDons { get; set; }

    }
}
