using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Enums;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
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
    public class AppRoleController : BaseController
    {
        private readonly IAppRoleApiClient _appRoleApiClient;
        private readonly IMenuApiClient _menuApiClient;
        public AppRoleController(IAppRoleApiClient appRoleApiClient, IMenuApiClient menuApiClient)
        {
            _appRoleApiClient = appRoleApiClient;
            _menuApiClient = menuApiClient;
        }

        public async Task<IActionResult> Index()
        {
            AppRoleViewModel model = new AppRoleViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Phân quyền";
            model.Breadcrumbs = new List<string>() { "Cài đặt", "Quản trị hệ thống", "Phân quyền" };

            var menuApiClient = await _menuApiClient.GetAll(new ManageMenuPagingRequest());

            if (menuApiClient.IsSuccessed)
            {
                model.HTMLMenu = GenerateHtmlMenu(menuApiClient.ResultObj);
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

            var request = new ManageAppRolePagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc"
            };

            var appRoleApiClient = await _appRoleApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = appRoleApiClient.TotalRecords,
                recordsTotal = appRoleApiClient.TotalRecords,
                data = appRoleApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] AppRoleDeleteRequest request)
        {
            return Json(await _appRoleApiClient.DeleteByIds(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditAppRoleRequest<AppRoleRequest> rq)
        {
            ApiResult<string> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));

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
                result = await _appRoleApiClient.AddOrUpdateAsync(rq.Data);
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

        public async Task<IActionResult> Filter(string textSearch)
        {
            var request = new ManageAppRolePagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc"
            };

            var appUserStatusApiClient = await _appRoleApiClient.GetManageListPaging(request);
            return Ok(appUserStatusApiClient);
        }
        private string GenerateHtmlMenu(List<MenuDto> menus)
        {
            string result = string.Empty;

            foreach (var menu in menus.Where(m => m.ParentId == null).OrderBy(m => m.SortOrder))
            {
                result += GenerateHtmlMenuNode(menu, menus);
            }

            return result;
        }
        private string GenerateHtmlMenuNode(MenuDto currentMenu, List<MenuDto> menus)
        {
            string str = string.Empty;

            string eleId = "modal_edit_role_assign_menu_" + currentMenu.Id.ToString();
            //begin::Section
            str += $@"<div class='m-0'>";
            //begin::Heading
            str += $@"<div class='d-flex align-items-center collapsible py-3 toggle mb-0' data-bs-toggle='collapse' data-bs-target='#" + eleId + "'>";
            //begin::Icon
            str += $@"<div class='btn btn-sm btn-icon mw-20px btn-active-color-primary me-5'>";
            //begin::Svg Icon | path: icons/duotone/Interface/Minus-Square.svg
            str += $@"<span class='svg-icon toggle-on svg-icon-primary svg-icon-1'>";
            str += $@"<svg xmlns = 'http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24' fill='none'>";
            str += $@"<path opacity='0.25' d='M6.54184 2.36899C4.34504 2.65912 2.65912 4.34504 2.36899 6.54184C2.16953 8.05208 2 9.94127 2 12C2 14.0587 2.16953 15.9479 2.36899 17.4582C2.65912 19.655 4.34504 21.3409 6.54184 21.631C8.05208 21.8305 9.94127 22 12 22C14.0587 22 15.9479 21.8305 17.4582 21.631C19.655 21.3409 21.3409 19.655 21.631 17.4582C21.8305 15.9479 22 14.0587 22 12C22 9.94127 21.8305 8.05208 21.631 6.54184C21.3409 4.34504 19.655 2.65912 17.4582 2.36899C15.9479 2.16953 14.0587 2 12 2C9.94127 2 8.05208 2.16953 6.54184 2.36899Z' fill='#12131A' />";
            str += $@"<path d='M8 13C7.44772 13 7 12.5523 7 12C7 11.4477 7.44772 11 8 11H16C16.5523 11 17 11.4477 17 12C17 12.5523 16.5523 13 16 13H8Z' fill='#12131A' />";
            str += $@"</svg>";
            str += $@"</span>";
            //end::Svg Icon
            //begin::Svg Icon | path: icons / duotone / Interface / Plus - Square.svg
            str += $@"<span class='svg-icon toggle-off svg-icon-1'>";
            str += $@"<svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' viewBox='0 0 24 24' fill='none'>";
            str += $@"<path opacity='0.25' fill-rule='evenodd' clip-rule='evenodd' d='M6.54184 2.36899C4.34504 2.65912 2.65912 4.34504 2.36899 6.54184C2.16953 8.05208 2 9.94127 2 12C2 14.0587 2.16953 15.9479 2.36899 17.4582C2.65912 19.655 4.34504 21.3409 6.54184 21.631C8.05208 21.8305 9.94127 22 12 22C14.0587 22 15.9479 21.8305 17.4582 21.631C19.655 21.3409 21.3409 19.655 21.631 17.4582C21.8305 15.9479 22 14.0587 22 12C22 9.94127 21.8305 8.05208 21.631 6.54184C21.3409 4.34504 19.655 2.65912 17.4582 2.36899C15.9479 2.16953 14.0587 2 12 2C9.94127 2 8.05208 2.16953 6.54184 2.36899Z' fill='#12131A' />";
            str += $@"<path fill-rule='evenodd' clip-rule='evenodd' d='M12 17C12.5523 17 13 16.5523 13 16V13H16C16.5523 13 17 12.5523 17 12C17 11.4477 16.5523 11 16 11H13V8C13 7.44772 12.5523 7 12 7C11.4477 7 11 7.44772 11 8V11H8C7.44772 11 7 11.4477 7 12C7 12.5523 7.44771 13 8 13H11V16C11 16.5523 11.4477 17 12 17Z' fill='#12131A' />";
            str += $@"</svg>";
            str += $@"</span>";
            //end::Svg Icon
            str += $@"</div>";
            //end::Icon
            //begin::Title
            str += $@"<h4 class='text-gray-700 fw-bolder cursor-pointer mb-0'>" + currentMenu.Name + "</h4>";
            //end::Title
            str += $@"</div>";
            //end::Heading
            //begin::Body
            str += $@"<div id='" + eleId + "' class='collapse show fs-6 ms-1'>";
            //begin::Text
            str += $@"<div class='mb-4 text-gray-600 fw-bold fs-6 ps-10'>";
            //begin::Wrapper
            //begin::Table wrapper
            str += $@"<div class='table-responsive'>";
            //begin::Table
            str += $@"<table class='table align-middle table-row-dashed fs-6 gy-5'>";
            //begin::Table body
            str += $@"<tbody class='text-gray-600 fw-bold'>";
            str += $@"<tr>";
            str += $@"<td>";

            //end::Checkbox
            str += $@"</td>";
            str += $@"</tr>";
            str += $@"<tr>";
            str += $@"<td>";
            //begin::Checkbox
            str += $@"<label class='form-check form-check-sm form-check-custom form-check-solid me-5 me-lg-20'>";
            str += $@"<input class='form-check-input' type='checkbox' value='' data-menuid='" + currentMenu.Id.ToString() + "' data-menuapproletype='" + ((int)MenuAppRoleType.SystemDataView).ToString() + "' name='modal_edit_role_assign_checkbox' />";
            str += $@"<span class='form-check-label'>Xem dữ liệu hệ thống</span>";
            str += $@"</label>";
            //end::Checkbox
            str += $@"</td>";
            str += $@"<td>";
            //begin::Checkbox-->
            str += $@"<label class='form-check form-check-custom form-check-solid me-5 me-lg-20'>";
            str += $@"<input class='form-check-input' type='checkbox' value='' data-menuid='" + currentMenu.Id.ToString() + "' data-menuapproletype='" + ((int)MenuAppRoleType.SystemDataEdit).ToString() + "' name='modal_edit_role_assign_checkbox' />";
            str += $@"<span class='form-check-label'>Chỉnh sửa dữ liệu hệ thống</span>";
            str += $@"</label>";
            //end::Checkbox
            str += $@"</td>";
            str += $@"<td>";
            //begin::Checkbox
            str += $@"<label class='form-check form-check-custom form-check-solid'>";
            str += $@"<input class='form-check-input' type='checkbox' value='' data-menuid='" + currentMenu.Id.ToString() + "' data-menuapproletype='" + ((int)MenuAppRoleType.SystemDataDelete).ToString() + "' name='modal_edit_role_assign_checkbox' />";
            str += $@"<span class='form-check-label'>Xóa dữ liệu hệ thống</span>";
            str += $@"</label>";
            //end::Checkbox
            str += $@"</td>";
            str += $@"</tr>";
            str += $@"<tr>";
            str += $@"<td>";
            //begin::Checkbox
            str += $@"<label class='form-check form-check-custom form-check-solid'>";
            str += $@"<input class='form-check-input' type='checkbox' value='' data-menuid='" + currentMenu.Id.ToString() + "' data-menuapproletype='" + ((int)MenuAppRoleType.DownloadExcel).ToString() + "' name='modal_edit_role_assign_checkbox' />";
            str += $@"<span class='form-check-label'>Tải excel hệ thống</span>";
            str += $@"</label>";
            //end::Checkbox
            str += $@"</td>";
            str += $@"</tr>";
            str += $@"</tbody>";
            str += $@" </table>";
            str += $@"</div>";
            //end::Table wrapper

            foreach (var subMenu in menus.Where(m => m.ParentId != null && m.ParentId == currentMenu.Id).OrderBy(m => m.SortOrder))
            {
                str += GenerateHtmlMenuNode(subMenu, menus);
            }

            str += $@"</div>";
            //end::Text
            str += $@"</div>";
            //end::Body
            //begin::Separator
            str += $@"<div class='separator separator-dashed'></div>'";
            //end::Separator
            str += $@"</div>";
            //end::Section

            return str;
        }
    }
}
