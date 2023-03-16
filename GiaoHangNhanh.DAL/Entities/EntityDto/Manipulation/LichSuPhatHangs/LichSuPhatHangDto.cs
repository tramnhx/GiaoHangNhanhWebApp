using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuPhatHangs
{
    public class LichSuPhatHangDto : BaseEntity
    {
        public string StrCreatedDay { get; set; }
        public VanDonDto VanDon { get; set; }
        public NhanVienDto NhanVien { get; set; } 
    }
}