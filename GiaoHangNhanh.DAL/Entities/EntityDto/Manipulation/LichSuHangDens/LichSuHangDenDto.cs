using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuHangDens
{
    public class LichSuHangDenDto : BaseDto
    {
        public string StrCreatedDay { get; set; }
        public BuuCucDto BuuCuc { get; set; }
        public VanDonDto VanDon { get; set; }
    }
}
