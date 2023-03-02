using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;


namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs
{
    public class CongTyGuiHangDto : BaseDto
    {
        public string PhoneNumber { get; set; }
        public string DiaChi { get; set; }
        public TinhDto Tinh { get; set; }
        public HuyenDto Huyen { get; set; }
        public KhuVucDto KhuVuc { get; set; }
    }
}
