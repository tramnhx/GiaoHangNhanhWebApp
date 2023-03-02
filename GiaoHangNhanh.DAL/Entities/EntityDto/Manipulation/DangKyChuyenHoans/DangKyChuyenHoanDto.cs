using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhanLoaiHangBatThuongs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuGuiHangs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans
{
    public class DangKyChuyenHoanDto : BaseDto
    {
        public string NguyenNhan { get; set; }
        public string MieuTaNguyenNhan { get; set; }
        public VanDonDto VanDon { get; set; }

    }
}
