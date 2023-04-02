using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DuyetChuyenHoans;
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
    public class DuyetChuyenHoanController : BaseController
    {
        private readonly IDuyetChuyenHoanApiClient _duyetChuyenHoanApiClient;
        public DuyetChuyenHoanController(IDuyetChuyenHoanApiClient duyetChuyenHoanApiClient)
        {
            _duyetChuyenHoanApiClient = duyetChuyenHoanApiClient;
        }
        public IActionResult Index()
        {
            var model = new DuyetChuyenHoanViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Thao tác duyệt chuyển hoàn";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Thao tác hệ thống", "Duyệt Chuyển Hoàn" };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch, string filterByDangKyChuyenHoanId, string filterByVanDonId)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            int filterByDangKyChuyenHoanIdValue, filterByVanDonIdValue;

            var request = new ManageDuyetChuyenHoanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
                FilterByDangKyChuyenHoanId = int.TryParse(filterByDangKyChuyenHoanId, out filterByDangKyChuyenHoanIdValue) ? filterByDangKyChuyenHoanIdValue : new Nullable<int>(),
                FilterByVanDonId = int.TryParse(filterByVanDonId, out filterByVanDonIdValue) ? filterByVanDonIdValue : new Nullable<int>()

            };

            var duyetChuyenHoanApiClient = await _duyetChuyenHoanApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = duyetChuyenHoanApiClient.TotalRecords,
                recordsTotal = duyetChuyenHoanApiClient.TotalRecords,
                data = duyetChuyenHoanApiClient.Items
            });
        }

        //[HttpPost]
        //public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        //{
        //    return Json(await _duyetChuyenHoanApiClient.DeleteByIds(request));
        //}

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<DuyetChuyenHoanRequest> rq)
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
                result = await _duyetChuyenHoanApiClient.AddOrUpdateAsync(rq.Data);
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
            var request = new ManageDuyetChuyenHoanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc"
            };

            var data = await _duyetChuyenHoanApiClient.GetManageListPaging(request);
            return Ok(data);
        }
    }
}
