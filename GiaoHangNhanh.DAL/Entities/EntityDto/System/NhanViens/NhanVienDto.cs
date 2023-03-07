using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Genders;
using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens
{
    public class NhanVienDto
    {
        public int? Id { set; get; }
        public string Code { set; get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string CCCD { get; set; }
        public string DiaChi { set; get; }
        public int? BuuCucLamViecId { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public DateTime? NgaySinh { get; set; }
        public DateTime? NgayLamViec { get; set; }
        public string NgayNghiViec { get; set; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public string NoiSinh { get; set; }
        public int? GenderId { get; set; }
        public bool IsActive { get; set; }
        public BuuCucDto BuuCuc { get; set; }
        public GenderDto Gender { get; set; }
        public HuyenDto Huyen { set; get; }
        public TinhDto Tinh { set; get; }
        public KhuVucDto KhuVuc { set; get; }
    }
}
