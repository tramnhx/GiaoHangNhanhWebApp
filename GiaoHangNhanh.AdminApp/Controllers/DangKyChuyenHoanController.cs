﻿using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans;
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
    public class DangKyChuyenHoanController : BaseController
    {
        private readonly IDangKyChuyenHoanApiClient _dangKyChuyenHoanApiClient;
        public DangKyChuyenHoanController(IDangKyChuyenHoanApiClient dangKyChuyenHoanApiClient)
        {
            _dangKyChuyenHoanApiClient = dangKyChuyenHoanApiClient;
        }
        public IActionResult Index()
        {
            var model = new DangKyChuyenHoanViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Thao tác Đăng Ký Chuyển Hoàn";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Thao tác hệ thống", "Đăng Ký Chuyển Hoàn" };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
       
            var request = new ManageDangKyChuyenHoanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
               
            };

            var dangKyChuyenHoanApiClient = await _dangKyChuyenHoanApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = dangKyChuyenHoanApiClient.TotalRecords,
                recordsTotal = dangKyChuyenHoanApiClient.TotalRecords,
                data = dangKyChuyenHoanApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _dangKyChuyenHoanApiClient.DeleteByIds(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<DangKyChuyenHoanRequest> rq)
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
                result = await _dangKyChuyenHoanApiClient.AddOrUpdateAsync(rq.Data);
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
            var request = new ManageDangKyChuyenHoanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc"
            };

            var data = await _dangKyChuyenHoanApiClient.GetManageListPaging(request);
            return Ok(data);
        }
    }
}
