using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhuongThucThanhToans;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
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
    public class PhuongThucThanhToanController : BaseController
    {
        private readonly IPhuongThucThanhToanApiClient _phuongThucThanhToanApiClient;
        public PhuongThucThanhToanController(IPhuongThucThanhToanApiClient phuongThucThanhToanApiClient)
        {
            _phuongThucThanhToanApiClient = phuongThucThanhToanApiClient;
        }
        public IActionResult Index()
        {
            PhuongThucThanhToanViewModel model = new PhuongThucThanhToanViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Phương thức thanh toán";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Danh mục ", "Phương thức thanh toán" };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            var request = new ManagePhuongThucThanhToanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc"
            };

            var danhmucphuongthucthanhtoanApiClient = await _phuongThucThanhToanApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = danhmucphuongthucthanhtoanApiClient.TotalRecords,
                recordsTotal = danhmucphuongthucthanhtoanApiClient.TotalRecords,
                data = danhmucphuongthucthanhtoanApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _phuongThucThanhToanApiClient.DeleteByIds(request));
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<PhuongThucThanhToanRequest> rq)
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
                result = await _phuongThucThanhToanApiClient.AddOrUpdateAsync(rq.Data);
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

        [HttpGet]
        public async Task<IActionResult> Filter(string textSearch)
        {
            var request = new ManagePhuongThucThanhToanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc"
            };

            var data = await _phuongThucThanhToanApiClient.GetManageListPaging(request);
            return Ok(data);
        }
    }
}

