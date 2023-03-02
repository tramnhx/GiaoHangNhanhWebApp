using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Constants;
using GiaoHangNhanh.Utilities.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Controllers
{
    public class StaffController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IAppRoleApiClient _appRoleApiClient;
        private readonly IConfiguration _configuration;

        public StaffController(IUserApiClient userApiClient, IAppRoleApiClient appRoleApiClient,
                                    IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _appRoleApiClient = appRoleApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            StaffViewModel model = new StaffViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);
            model.PageTitle = "Nhân viên";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Nhân viên" };

            var appRoleApiClient = await _appRoleApiClient.GetAll(new ManageAppRolePagingRequest());

            if (appRoleApiClient.IsSuccessed)
            {
                model.AppRoles = appRoleApiClient.ResultObj.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList();
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;

            var request = new ManageUserPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc"
            };

            var userApiClient = await _userApiClient.GetStaffManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = userApiClient.TotalRecords,
                recordsTotal = userApiClient.TotalRecords,
                data = userApiClient.Items
            });
        }
        [HttpPost]
        public async Task<IActionResult> ChangeCurrentUserPassword([FromBody] ChangePasswordRequest request)
        {
            ApiResult<string> result = null;

            if (User.Identity.Name.Equals(request.UserName))
            {
                if (request != null)
                {
                    result = await _userApiClient.ChangePassword(request);
                }
                else
                {
                    result = new ApiErrorResult<string>("Không có dữ liệu");
                }
            }
            else
            {
                result = new ApiErrorResult<string>("Bạn không được phép đổi mật khẩu của User khác.");
            }


            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> SaveSecurity([FromBody] AddEditUserRequest<UserRequest> rq)
        {
            ApiResult<string> result = null;

            string userGuid = HttpContext.Session.GetString(SystemConstants.UserConstants.UserId);

            if (rq != null)
            {
                if (string.IsNullOrEmpty(rq.Id))
                {
                    rq.Data.CreatedUserId = userGuid;
                    rq.Data.ModifiedUserId = userGuid;
                }
                else
                {
                    rq.Data.ModifiedUserId = userGuid;
                    rq.Data.Id = rq.Id;
                }
                result = await _userApiClient.AddOrUpdateStaffSecurity(rq.Data);
            }
            else
            {
                result = new ApiResult<string>()
                {
                    IsSuccessed = false,
                    Message = "Không có dữ liệu"
                };
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> SecurityEdit(string id)
        {
            var model = new StaffViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Nhân viên";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Nhân viên" };
            model.FileNoImagePerson = _configuration[SystemConstants.AppConstants.BaseAddress] + _configuration[SystemConstants.AppConstants.FileNoImagePerson];

            if (id != null)
            {
                var userApiClient = await _userApiClient.GetStaffSecurityByUserId(new UserRequest
                {
                    Id = id
                });

                if (userApiClient.IsSuccessed)
                {
                    model.Staff = userApiClient.ResultObj;
                    model.Staff.FullName = model.Staff.FullName;
                    if (!string.IsNullOrEmpty(model.Staff.Avatar))
                    {
                        model.Staff.Avatar = _configuration[SystemConstants.AppConstants.BaseAddress] + "/" + model.Staff.Avatar;
                    }
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProfileDetail([FromForm] UserRequest rq)
        {
            ApiResult<string> result = null;

            string userGuid = HttpContext.Session.GetString(SystemConstants.UserConstants.UserId);

            if (rq != null)
            {
                if (string.IsNullOrEmpty(rq.Id))
                {
                    rq.CreatedUserId = userGuid;
                    rq.ModifiedUserId = userGuid;
                }
                else
                {
                    rq.ModifiedUserId = userGuid;
                    rq.Id = rq.Id;
                }

                result = await _userApiClient.AddOrUpdateStaffProfileDetail(rq);
            }
            else
            {
                result = new ApiErrorResult<string>("Không có dữ liệu");
            }

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> ProfileDetailEdit(string id)
        {
            var model = new StaffViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Nhân viên";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Nhân viên" };
            model.FileNoImagePerson = _configuration[SystemConstants.AppConstants.BaseAddress] + _configuration[SystemConstants.AppConstants.FileNoImagePerson];

            if (id != null)
            {
                var userApiClient = await _userApiClient.GetStaffProfileDetailByUserId(new UserRequest
                {
                    Id = id
                });

                if (userApiClient.IsSuccessed)
                {
                    model.Staff = userApiClient.ResultObj;
                    model.Staff.FullName = model.Staff.LastName + " " + model.Staff.FirstName;
                    if (!string.IsNullOrEmpty(model.Staff.Avatar))
                    {
                        model.Staff.Avatar = _configuration[SystemConstants.AppConstants.BaseAddress] + model.Staff.Avatar;
                    }
                }
                 var a = model.Staff.Address;
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteByIds([FromBody] UserDeleteRequest request)
        {
            return Json(await _userApiClient.DeleteByIds(request));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public async Task<IActionResult> Filter(string textSearch)
        {

            var request = new ManageUserPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc"
            };

            var data = await _userApiClient.GetStaffManageListPaging(request);
            return Ok(data);
        }
    }
}
