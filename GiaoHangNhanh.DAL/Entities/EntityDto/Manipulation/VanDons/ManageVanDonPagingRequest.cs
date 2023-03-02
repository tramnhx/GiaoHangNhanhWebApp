using GiaoHangNhanh.DAL.Entities.EntityDto.Common;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons
{
    public class ManageVanDonPagingRequest : PagingRequestBase
    {
        public int? FilterByBuuCucId { set; get; }
        public int? FilterByTinh { set; get; }
        public int? FilterByHuyen { set; get; }
        public int? FilterByKhuVuc { set; get; }
        public int? FilterByCongTyGuiHangId { set; get; }
        public int? FilterByPhuongThucThanhToanId { set; get; }
        public int? FilterByDichVuId { set; get; }

    }
}
