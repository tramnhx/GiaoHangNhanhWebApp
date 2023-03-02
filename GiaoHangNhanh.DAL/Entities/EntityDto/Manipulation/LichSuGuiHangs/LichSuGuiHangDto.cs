using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuGuiHangs
{
    public class LichSuGuiHangDto : BaseDto
    {
        public int? VanDonId { get; set; }
        public VanDonDto VanDon { get; set; }
        public BuuCucDto BuuCuc { get; set; }

    }
}
