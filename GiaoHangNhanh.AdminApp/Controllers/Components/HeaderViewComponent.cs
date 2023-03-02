using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
using GiaoHangNhanh.DAL.Entities.EntityDto.UI.AdminApp;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Controllers.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly IAdminAppUIApiClient _adminAppUIApiClient;
        private readonly IConfiguration _configuration;

        public HeaderViewComponent(IAdminAppUIApiClient adminAppUIApiClient, IConfiguration configuration)
        {
            _adminAppUIApiClient = adminAppUIApiClient;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string userId = HttpContext.Session.GetString(SystemConstants.UserConstants.UserId);
            var headerViewModel = new HeaderViewModel()
            {
                UserImage = _configuration[SystemConstants.AppConstants.BaseAddress] + _configuration[SystemConstants.AppConstants.FileNoImagePerson]
            };

            var adminAppUIApiClient = await _adminAppUIApiClient.GetHeaderObjects(new AdminAppHeaderRequest()
            {
                UserName = User.Identity.Name
            });
            if (adminAppUIApiClient.IsSuccessed)
            {
                headerViewModel.UserImage = _configuration[SystemConstants.AppConstants.BaseAddress] + adminAppUIApiClient.ResultObj.UserImage;
                headerViewModel.FullName = adminAppUIApiClient.ResultObj.FullName;
                headerViewModel.Email = adminAppUIApiClient.ResultObj.Email;

                var leftSideBarApiClient = await _adminAppUIApiClient.GetLeftSideBarObjects();
                headerViewModel.HtmlMenus = GenerateHtmlMenu(leftSideBarApiClient.ResultObj.Menus);
                headerViewModel.Logo = _configuration[SystemConstants.AppConstants.BaseAddress] + leftSideBarApiClient.ResultObj.Logo;
                HttpContext.Session.SetString(SystemConstants.AppConstants.Logo, _configuration[SystemConstants.AppConstants.BaseAddress] + leftSideBarApiClient.ResultObj.Logo);

            }

            return View("Default", headerViewModel);
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

            if (menus.Count(m => m.ParentId != null && m.ParentId == currentMenu.Id) > 0)
            {

                str += $@"<div data-kt-menu-trigger='click' data-kt-menu-placement='bottom-start' class='menu-item menu-lg-down-accordion me-lg-1'>";
                str += $@"<span class='menu-link py-3'>";
                str += $@"<span class='menu-title'>" + currentMenu.Name + "</span>";
                str += $@"<span class='menu-arrow d-lg-none'></span>";
                str += $@"</span>";
                str += $@"<div class='menu-sub menu-sub-lg-down-accordion menu-sub-lg-dropdown menu-rounded-0 py-lg-4 w-lg-225px'>";

                foreach (var subMenu in menus.Where(m => m.ParentId != null && m.ParentId == currentMenu.Id).OrderBy(m => m.SortOrder))
                {
                    str += GenerateHtmlMenuNode(subMenu, menus);
                }
                str += $@"</div>";
                str += $@"</div>";
            }
            else
            {
                str += $@"<div data-kt-menu-trigger='{default:'click', lg: 'hover'}' data-kt-menu-placement='right-start' class='menu-item menu-lg-down-accordion'>";
                str += $@"<a class='menu-link py-3' href='" + currentMenu.Link + "'>";
                if (currentMenu.ParentId != null)
                {
                    str += $@"<span class='menu-bullet'>";
                    str += $@"<span class='bullet bullet-dot'></span>";
                    str += $@"</span>";
                }

                str += $@"<span class='menu-title'>" + currentMenu.Name + "</span>";
                str += $@"</a>";
                str += $@"</div>";
            }

            return str;
        }
    }
}
