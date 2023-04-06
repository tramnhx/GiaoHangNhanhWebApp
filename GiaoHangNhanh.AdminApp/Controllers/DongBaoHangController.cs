using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs;
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
    public class DongBaoHangController : BaseController
    {
        private readonly ILichSuBaoHangApiClient _lichSuBaoHangApiClient;
        public DongBaoHangController(ILichSuBaoHangApiClient lichSuBaoHangApiClient)
        {
            _lichSuBaoHangApiClient = lichSuBaoHangApiClient;
        }
        public IActionResult Index()
        {
            var model = new LichSuBaoHangViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Lịch sử bao hàng";
            model.Breadcrumbs = new List<string>() { "Thao tác", "Bao hàng" };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            var request = new ManageLichSuBaoHangPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
            };
            var userApiClient = await _lichSuBaoHangApiClient.GetManageBaoHangListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = userApiClient.TotalRecords,
                recordsTotal = userApiClient.TotalRecords,
                data = userApiClient.Items
            });
        }
        [HttpPost]
        public async Task<IActionResult> DataTableVanDonInBaoGetList(int? draw, int? start, int? length, string textSearch
            , string filterByMaSealBao)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            var request = new ManageLichSuBaoHangPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
                FilterByMaSealBao = filterByMaSealBao != null ? filterByMaSealBao : "0",
            };
            var lichSuBaoHang = await _lichSuBaoHangApiClient.GetManageVanDonInBaoListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = lichSuBaoHang.TotalRecords,
                recordsTotal = lichSuBaoHang.TotalRecords,
                data = lichSuBaoHang.Items
            });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));
            var model = new LichSuBaoHangViewModel();

            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);
            model.PageTitle = "Bao hàng";
            model.Breadcrumbs = new List<string>() { "Thao tác", "Lịch sử bao hàng" };

            if (id != 0)
            {
                var lichSuBaoHangApiClient = await _lichSuBaoHangApiClient.GetById(id);

                if (lichSuBaoHangApiClient.IsSuccessed)
                {
                    model.LichSuBaoHang = lichSuBaoHangApiClient.ResultObj;
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<LichSuBaoHangRequest> rq)
        {
            ApiResult<int> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));

            if (rq != null)
            {
                if (rq.Id == null)
                {
                    rq.Data.CreatedUserId = userGuid;
                    result = await _lichSuBaoHangApiClient.DongBaoHangAsync(rq.Data);
                    var vandonInBao = new VanDonInBaoRequest()
                    {
                        BaoHangId = result.ResultObj,
                        VanDonIds = rq.Data.VanDonIds,
                        CreatedUserId = rq.Data.CreatedUserId,
                    };
                    await SaveVanDonInBao(vandonInBao);
                }
                else
                {
                    var vandon = new VanDonInBaoRequest()
                    {
                        BaoHangId = rq.Id,
                        VanDonIds = rq.Data.VanDonIds,
                        CreatedUserId = rq.Data.CreatedUserId,
                    };
                    await SaveVanDonInBao(vandon);
                    return Ok(result);
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
        
        [HttpPost]
        public async Task<IActionResult> SaveVanDonInBao([FromBody] VanDonInBaoRequest rq)
        {
            ApiResult<int> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));

            if (rq != null)
            {
                if (rq.VanDonIds != null)
                {
                    rq.CreatedUserId = userGuid;
                }
                result = await _lichSuBaoHangApiClient.AddVanDonInBaoAsync(rq);
            }

            return Ok(result);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _lichSuBaoHangApiClient.DeleteByIds(request));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteBaoHangByIds([FromBody] DeleteRequest request)
        {
            return Json(await _lichSuBaoHangApiClient.DeleteBaoHangByIds(request));
        }
        [HttpGet]
        public async Task<IActionResult> Filter(string textSearch)
        {
            var request = new ManageLichSuBaoHangPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc"
            };

            var data = await _lichSuBaoHangApiClient.GetManageBaoHangListPaging(request);
            return Ok(data);
        }
    }
}
