using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhanLoaiHangBatThuongs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyKienVanDes
{
    public class DangKyKienVanDeDto : BaseEntity
    {
        public string MieuTaVanDe { get; set; }
        public string QuaTrinhXuLy { get; set; }
        public VanDonDto VanDon { get; set; }
        public PhanLoaiHangBatThuongDto PhanLoaiHangBatThuong { get; set; }
    }
}
