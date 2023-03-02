using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
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
    public class BuuCucController : Controller
    {
        private readonly IBuuCucApiClient _buuCucApiClient;

        public BuuCucController(IBuuCucApiClient buuCucApiClient)
        {
            _buuCucApiClient = buuCucApiClient;
        }
        public IActionResult Index()
        {
            var model = new BuuCucViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Bưu Cục";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Danh mục hệ thống", "Bưu Cục" };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch, string filterByTinhId, string filterByHuyenId, string filterByKhuVucId)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            int filterByTinhIdValue, filterByHuyenIdValue, filterByKhuVucIdValue;

            var request = new ManageBuuCucPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
                FilterByTinhId = int.TryParse(filterByTinhId, out filterByTinhIdValue) ? filterByTinhIdValue : new Nullable<int>(),
                FilterByHuyenId = int.TryParse(filterByHuyenId, out filterByHuyenIdValue) ? filterByHuyenIdValue : new Nullable<int>(),
                FilterByKhuVucId = int.TryParse(filterByKhuVucId, out filterByKhuVucIdValue) ? filterByKhuVucIdValue : new Nullable<int>()
            };

            var buuCucApiClient = await _buuCucApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = buuCucApiClient.TotalRecords,
                recordsTotal = buuCucApiClient.TotalRecords,
                data = buuCucApiClient.Items
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _buuCucApiClient.DeleteByIds(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetByKhuVucId(int khuVucId)
        {
            var data = await _buuCucApiClient.GetByKhuVucId(khuVucId);
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<BuuCucRequest> rq)
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
                result = await _buuCucApiClient.AddOrUpdateAsync(rq.Data);
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
        public async Task<IActionResult> Filter(string textSearch, int? filterByKhuVucId)
        {
            var request = new ManageBuuCucPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc",
                FilterByKhuVucId = filterByKhuVucId
            };

            var data = await _buuCucApiClient.GetManageListPaging(request);
            return Ok(data);
        }
    }
}
