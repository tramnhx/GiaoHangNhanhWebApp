using BHSNetCoreLib.ExcelUtil;
using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuNhapKhos;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Constants;
using GiaoHangNhanh.Utilities.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Controllers
{
    public class LichSuNhapKhoController : BaseController
    {
        private readonly ILichSuNhapKhoApiClient _lichSuNhapKhoApiClient;
        public LichSuNhapKhoController(ILichSuNhapKhoApiClient lichSuNhapKhoApiClient)
        {
            _lichSuNhapKhoApiClient = lichSuNhapKhoApiClient;
        }
        public IActionResult Index()
        {
            LichSuNhapKhoViewModel model = new LichSuNhapKhoViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Lịch Sử Nhập Kho";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Thao Tác ", "Lịch Sử Nhập Kho" };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            var request = new ManageLichSuNhapKhoPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc"
            };

            var lichsunhapkhoApiClient = await _lichSuNhapKhoApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = lichsunhapkhoApiClient.TotalRecords,
                recordsTotal = lichsunhapkhoApiClient.TotalRecords,
                data = lichsunhapkhoApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _lichSuNhapKhoApiClient.DeleteByIds(request));
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<LichSuNhapKhoRequest> rq)
        {
            ApiResult<int> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));

            if (rq != null)
            {
                if (rq.Id == null)
                {
                    rq.Data.CreatedUserId = userGuid;
                    rq.Data.ModifiedUserId = userGuid;
                }
                else
                {
                    rq.Data.ModifiedUserId = userGuid;
                    rq.Data.Id = rq.Id.Value;
                }
                result = await _lichSuNhapKhoApiClient.AddOrUpdateAsync(rq.Data);
            }
            else
            {
                result = new ApiResult<int>()
                {
                    IsSuccessed = false,
                    Message = "Không có dữ liệu"
                };
            }

            return Ok(result);
        }
      
        public async Task<IActionResult> Filter(string textSearch, int filterByBuuCucGuiHangId, int filterByUserId)
        {
            var request = new ManageLichSuNhapKhoPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc",
                FilterByBuuCucGuiHangId = filterByBuuCucGuiHangId,
                FilterByUserId = filterByUserId
            };

            var lichsunhapkhoApiClient = await _lichSuNhapKhoApiClient.GetManageListPaging(request);
            return Ok(lichsunhapkhoApiClient);
        }
    }
}
