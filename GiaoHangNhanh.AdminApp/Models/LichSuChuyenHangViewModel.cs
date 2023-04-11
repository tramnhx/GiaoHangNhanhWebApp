using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuChuyenHangs;

namespace GiaoHangNhanh.AdminApp.Models
{
    public class LichSuChuyenHangViewModel : BaseViewModel
    {
        public LichSuChuyenHangDto LichSuChuyenHang { get; set; }
        public bool IsXeDi { get; set; }
    }
}
