using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Models
{
    public class LichSuBaoHangViewModel : BaseViewModel
    {
        public LichSuBaoHangDto LichSuBaoHang { get; set; }
        public List<SelectListItem> DonHangs { get; set; }
    }
}
