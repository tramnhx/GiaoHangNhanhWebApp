﻿using GiaoHangNhanh.DAL.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyKienVanDes
{
    public class DangKyKienVanDeRequest : BaseEntity
    {
        public int? Id { get; set; }
        public int? VanDonId { get; set; }
        public int? PhanLoaiHangBatThuongId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifieDate { get; set; }
        public string CreatedUserId { set; get; }
        public string ModifiedUserId { set; get; }
    }
}
