using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans
{
    public class KyNhanDto : BaseDto
    {
        public int? BuuCucId { get; set; }
        public int? VanDonId { get; set; }
        public Guid NhanVienPhat { get; set; }
        public string TenNguoiKy { get; set; }
        public bool DauKyThay { get; set; }
        public string NgayKyNhan { get; set; }
        public BuuCucDto BuuCuc { get; set; }
        public VanDonDto VanDon { get; set; }
        public AppUser AppUser { get; set; }
    }
}
