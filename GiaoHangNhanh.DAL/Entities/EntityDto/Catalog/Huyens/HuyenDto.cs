using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens
{
    public class HuyenDto : BaseDto
    {
        public TinhDto Tinh { get; set; }
    }
}
