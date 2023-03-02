using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;


namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs
{
    public class BuuCucDto : BaseDto
    {
        public TinhDto Tinh { get; set; }
        public HuyenDto Huyen { get; set; }
        public KhuVucDto KhuVuc { get; set; }
        public string DiaChi { get; set; }
    }
}
