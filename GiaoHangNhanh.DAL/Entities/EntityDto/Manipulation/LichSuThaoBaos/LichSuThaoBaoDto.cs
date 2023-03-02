using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuThaoBaos
{
    public class LichSuThaoBaoDto : BaseDto
    {
        public string SealBao { get; set; }
        public VanDonDto VanDon { get; set; }
    }
}
