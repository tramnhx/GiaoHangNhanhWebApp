using GiaoHangNhanh.AdminApp;
using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Constants;
using GiaoHangNhanh.Utilities.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGShipperWebApp.AdminApp.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly INhanVienApiClient _nhanVienApiClient;
        public NhanVienController(INhanVienApiClient nhanVienApiClient)
        {
            _nhanVienApiClient = nhanVienApiClient;
        }
        public IActionResult Index()
        {
            NhanVienViewModel model = new NhanVienViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Nhân viên";
            model.Breadcrumbs = new List<string>() { "Quản trị hệ thống", "Nhân viên" };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch,
            string filterByGenderId, string filterByBuuCucId)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            int filterByGenderIdValue = 0;
            int filterByBuuCucIdValue = 0;
            var request = new ManageNhanVienPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
                FilterByGenderId = int.TryParse(filterByGenderId, out filterByGenderIdValue) ? filterByGenderIdValue : new Nullable<int>(),
                FilterByBuuCucLamViecId = int.TryParse(filterByBuuCucId, out filterByBuuCucIdValue) ? filterByBuuCucIdValue : new Nullable<int>()
            };
            var a = request.FilterByBuuCucLamViecId;
            var userApiClient = await _nhanVienApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = userApiClient.TotalRecords,
                recordsTotal = userApiClient.TotalRecords,
                data = userApiClient.Items
            });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));
            NhanVienViewModel model = new NhanVienViewModel();

            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);
            model.PageTitle = "Nhân viên";
            model.Breadcrumbs = new List<string>() { "Quản trị hệ thống", "Nhân viên" };

            if (id != 0)
            {
                var nhanvienApiClient = await _nhanVienApiClient.GetById(id);

                if (nhanvienApiClient.IsSuccessed)
                {
                    model.NhanVien = nhanvienApiClient.ResultObj;
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _nhanVienApiClient.DeleteByIds(request));
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<NhanVienRequest> rq)
        {
            ApiResult<int> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));


            if (rq != null)
            {
                if (rq.Id == null)
                {
                    rq.Data.CreatedUserId = userGuid;
                }
                else
                {
                    rq.Data.ModifiedUserId = userGuid;
                    rq.Data.Id = rq.Id.Value;
                }
                result = await _nhanVienApiClient.AddOrUpdateAsync(rq.Data);
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
            var request = new ManageNhanVienPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc",
            };

            var nhanVienApiClient = await _nhanVienApiClient.GetManageListPaging(request);
            return Ok(nhanVienApiClient);
        }
    }
}
