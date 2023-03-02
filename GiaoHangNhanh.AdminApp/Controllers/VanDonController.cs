using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
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
    public class VanDonController : BaseController
    {
        private readonly IVanDonApiClient _vanDonApiClient;
        public VanDonController(IVanDonApiClient vanDonApiClient)
        {
            _vanDonApiClient = vanDonApiClient;
        }

        public IActionResult Index()
        {
            var model = new VanDonViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);
            model.PageTitle = "Vận đơn";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Thao Tác", "Vận đơn" };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            //Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));
            var model = new VanDonViewModel();

            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);
            model.PageTitle = "Thêm mới vận đơn";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Thao Tác", "Vận đơn", "Thêm mới" };

            if (id != 0)
            {
                var vanDonApiClient = await _vanDonApiClient.GetById(id);

                if (vanDonApiClient.IsSuccessed)
                {
                    model.VanDon = vanDonApiClient.ResultObj;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch, string filterByCongTyGuiHangId, string filterByDichVuId, string filterByPhuongThucThanhToanId, string filterByBuuCucId)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            // int filterByCongTyGuiHangIdValue, filterByDichVuIdValue, filterByBuuCucIdValue, filterByPhuongThucThanhToanIdValue;

            var request = new ManageVanDonPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
                //FilterByCongTyGuiHangId = int.TryParse(filterByCongTyGuiHangId, out filterByCongTyGuiHangIdValue) ? filterByCongTyGuiHangIdValue : new Nullable<int>(),
                //FilterByDichVuId = int.TryParse(filterByDichVuId, out filterByDichVuIdValue) ? filterByDichVuIdValue : new Nullable<int>(),
                //FilterByBuuCucId = int.TryParse(filterByBuuCucId, out filterByBuuCucIdValue) ? filterByBuuCucIdValue : new Nullable<int>(),
                //FilterByPhuongThucThanhToanId = int.TryParse(filterByPhuongThucThanhToanId, out filterByPhuongThucThanhToanIdValue) ? filterByPhuongThucThanhToanIdValue : new Nullable<int>(),
            };

            var congTyGuiHangApiClient = await _vanDonApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = congTyGuiHangApiClient.TotalRecords,
                recordsTotal = congTyGuiHangApiClient.TotalRecords,
                data = congTyGuiHangApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _vanDonApiClient.DeleteByIds(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetByCode(string code)
        {
            return Json(await _vanDonApiClient.GetByCode(code));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int? id)
        {
            return Json(await _vanDonApiClient.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<VanDonRequest> rq)
        {
            ApiResult<int> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));

            if (rq != null)
            {
                if (rq.Id == null)
                {
                    rq.Data.CreatedUserId = userGuid.ToString();
                }
                else
                {
                    rq.Data.ModifiedUserId = userGuid.ToString();
                    rq.Data.Id = rq.Id.Value;
                }
                result = await _vanDonApiClient.AddOrUpdateAsync(rq.Data);
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
        public async Task<IActionResult> Filter(string textSearch, int? id)
        {
            var request = new ManageVanDonPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc",
            };

            var data = await _vanDonApiClient.GetManageListPaging(request);
            return Ok(data);
        }

    }
}
