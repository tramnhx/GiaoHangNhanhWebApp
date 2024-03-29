﻿using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs
{
    public class BuuCucRequest
    {
        public int? Id { set; get; }
        public int SortOrder { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string DateString { set; get; }
        public string Description { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ModifiedDate { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
        public bool IsDefault { set; get; }
        public int TinhId { get; set; }
        public int HuyenId { get; set; }
        public int KhuVucId { get; set; }
    }
}
