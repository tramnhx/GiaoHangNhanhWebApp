using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuThaoBaos;
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
    public class LichSuThaoBaoController : BaseController
    {

        private readonly ILichSuThaoBaoApiClient _LichSuThaoBaoApiClient;
        public LichSuThaoBaoController(ILichSuThaoBaoApiClient LichSuThaoBaoApiClient)
        {
            _LichSuThaoBaoApiClient = LichSuThaoBaoApiClient;
        }
        public IActionResult Index()
        {
            LichSuThaoBaoViewModel model = new LichSuThaoBaoViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Lịch Sử Tháo Bao";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Danh mục hệ thống", "Lịch Sử Tháo Bao" };

            return View(model);
        }
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            var request = new ManageLichSuThaoBaoPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc"
            };

            var LichSuThaoBaoApiClient = await _LichSuThaoBaoApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = LichSuThaoBaoApiClient.TotalRecords,
                recordsTotal = LichSuThaoBaoApiClient.TotalRecords,
                data = LichSuThaoBaoApiClient.Items
            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _LichSuThaoBaoApiClient.DeleteByIds(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<LichSuThaoBaoRequest> rq)
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
                result = await _LichSuThaoBaoApiClient.AddOrUpdateAsync(rq.Data);
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
    }
}
