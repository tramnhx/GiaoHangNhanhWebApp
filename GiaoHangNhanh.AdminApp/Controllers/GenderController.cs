using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Genders;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Constants;
using GiaoHangNhanh.Utilities.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GiaoHangNhanh.AdminApp.Controllers
{
    public class GenderController : BaseController
    {
        private readonly IGenderApiClient _genderApiClient;
        public GenderController(IGenderApiClient genderApiClient)
        {
            _genderApiClient = genderApiClient;
        }

        public IActionResult Index()
        {
            var model = new GenderViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Giới tính";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Danh mục hệ thống", "Giới tính" };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            
            var request = new ManageGenderPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc"
            };

            var genderApiClient = await _genderApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = genderApiClient.TotalRecords,
                recordsTotal = genderApiClient.TotalRecords,
                data = genderApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _genderApiClient.DeleteByIds(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<GenderRequest> rq)
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
                result = await _genderApiClient.AddOrUpdateAsync(rq.Data);
                int a = result.ResultObj;
                if (a == 3)
                {
                    result = new ApiResult<int>()
                    {
                        IsSuccessed = false,
                        Message = "Không có dữ liệu"
                    };
                }
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
            var request = new ManageGenderPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc"
            };

            var data = await _genderApiClient.GetManageListPaging(request);
            return Ok(data);
        }
    }
}
