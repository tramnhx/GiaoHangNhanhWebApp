using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs
{
    public class KhuVucDto : BaseDto
    {
        public HuyenDto Huyen { get; set; }
        public TinhDto Tinh { get; set; }
    }
}
