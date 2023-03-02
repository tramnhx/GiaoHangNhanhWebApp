using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDis
{
    public class LichSuXeDiDto : BaseDto
    {
        public VanDonDto VanDon { get; set; }
        public string SealXe { get; set; }
        public BuuCucDto BuuCuc { get; set; }
        public string SealBao { get; set; }
    }
}
