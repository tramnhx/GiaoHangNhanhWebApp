namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class LichSuHangDen : BaseEntity
    {
        public int? VanDonId { get; set; }
        public VanDon VanDon { get; set; }
        public int? BuuCucId { get; set; }
        public BuuCuc BuuCuc { get; set; }
    }
}
