using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;


namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuChuyenHangs
{
    public class LichSuChuyenHangDto : BaseDto
    {
        public string StrCreatedDay { get; set; }
        public string SealXe { get; set; }
        public string IsXeDi { get; set; }
        public string TenBuuCuc { get; set; }
    }
}
