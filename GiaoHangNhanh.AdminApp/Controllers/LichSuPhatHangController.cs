using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuPhatHangs;
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
    public class LichSuPhatHangController : BaseController
    {
        private readonly ILichSuPhatHangApiClient _lichSuPhatHangApiClient;
        public LichSuPhatHangController(ILichSuPhatHangApiClient lichSuPhatHangApiClient)
        {
            _lichSuPhatHangApiClient = lichSuPhatHangApiClient;
        }

        public IActionResult Index()
        {
            var model = new LichSuPhatHangViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);
            model.PageTitle = "Lịch sử phát hàng";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Thao Tác", "Lịch sử phát hàng" };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, 
                                                        string textSearch, string filterByCongTyGuiHangId, 
                                                        string filterByDichVuId, string filterByPhuongThucThanhToanId, 
                                                        string filterByBuuCucId)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            // int filterByCongTyGuiHangIdValue, filterByDichVuIdValue, filterByBuuCucIdValue, filterByPhuongThucThanhToanIdValue;

            var request = new ManageLichSuPhatHangPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
            };

            var congTyGuiHangApiClient = await _lichSuPhatHangApiClient.GetManageListPaging(request);

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
            return Json(await _lichSuPhatHangApiClient.DeleteByIds(request));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            return Json(await _lichSuPhatHangApiClient.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<LichSuPhatHangRequest> rq)
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
                result = await _lichSuPhatHangApiClient.AddOrUpdateAsync(rq.Data);
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
            var request = new ManageLichSuPhatHangPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc",
            };

            var data = await _lichSuPhatHangApiClient.GetManageListPaging(request);
            return Ok(data);
        }

    }
}