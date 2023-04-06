using Microsoft.AspNetCore.Identity;
using GiaoHangNhanh.DAL.Entities.Entity;
using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.Entity
{
    public class AppUser : IdentityUser<Guid>
    {
        public string MaNhanVien { get; set; }
        public bool IsDelete { set; get; }
        public string Code { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public DateTime? Dob { get; set; }
        public string Avatar { set; get; }
        public DateTime? ActivateDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsActive { set; get; }
        public DateTime StartingDate { set; get; }
        public DateTime? LeaveDate { set; get; }
        public string Address { set; get; }

        public ICollection<LichSuBaoHang> LichSuBaoHangs { get; set; }
        public ICollection<LichSuNhapKho> LichSuNhapKhos { get; set; }
        public ICollection<PhanLoaiVanDon> PhanLoaiHangBatThuongs { get; set; }
        public ICollection<VanDon> VanDons { get; set; }
    }
}
