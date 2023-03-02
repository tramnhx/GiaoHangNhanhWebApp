using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Controllers.Components
{
    public class LeftSideBarViewComponent : ViewComponent
    {
        private readonly IAdminAppUIApiClient _adminAppUIApiClient;
        private readonly IConfiguration _configuration;

        public LeftSideBarViewComponent(IAdminAppUIApiClient adminAppUIApiClient, IConfiguration configuration)
        {
            _adminAppUIApiClient = adminAppUIApiClient;
            _configuration = configuration;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var leftSideBarViewModel = new LeftSideBarViewModel();

            var leftSideBarApiClient = await _adminAppUIApiClient.GetLeftSideBarObjects();

            if (leftSideBarApiClient.IsSuccessed)
            {
                leftSideBarViewModel.HtmlMenus = GenerateHtmlMenu(leftSideBarApiClient.ResultObj.Menus);
                leftSideBarViewModel.Logo = _configuration[SystemConstants.AppConstants.BaseAddress] + leftSideBarApiClient.ResultObj.Logo;
            }

            return View("Default", leftSideBarViewModel);
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
        public static string GenerateHtmlMenuNode(MenuDto currentMenu, List<MenuDto> menus)
        {
            string str = string.Empty;

            if (menus.Count(m => m.ParentId != null && m.ParentId == currentMenu.Id) > 0)
            {

                str += $@"<div data-kt-menu-trigger='click' class='menu-item menu-accordion'>";
                str += $@"<span class='menu-link'>";
                //str += $@"<span class='menu-bullet'>";
                //str += $@"<span class='bullet bullet-dot'></span>";
                //str += $@"</span>";

                str += currentMenu.Icon;
                str += $@"<span class='menu-title'>" + currentMenu.Name + "</span>";
                str += $@"<span class='menu-arrow'></span>";
                str += $@"</span>";

                foreach (var subMenu in menus.Where(m => m.ParentId != null && m.ParentId == currentMenu.Id).OrderBy(m => m.SortOrder))
                {
                    str += $@"<div class='menu-sub menu-sub-accordion menu-active-bg'>";
                    str += GenerateHtmlMenuNode(subMenu, menus);
                    str += $@"</div>";
                }
                str += $@"</div>";
            }
            else
            {
                str += $@"<div class='menu-item'>";
                str += $@"<a class='menu-link' href='" + currentMenu.Link + "' data-menuid='" + currentMenu.Id.ToString() + "'>";
                str += $@"<span class='menu-bullet'>";
                str += $@"<span class='bullet bullet-dot'></span>";
                str += $@"</span>";
                str += $@"<span class='menu-title'>" + currentMenu.Name + "</span>";
                str += $@"</a>";
                str += $@"</div>";
            }

            return str;
        }
    }
}