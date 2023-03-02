using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Controllers
{
    public class MenuAppRoleController : BaseController
    {
        private readonly IMenuAppRoleApiClient _menuAppRoleApiClient;
        public MenuAppRoleController(IMenuAppRoleApiClient menuAppRoleApiClient)
        {
            _menuAppRoleApiClient = menuAppRoleApiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost()]
        public async Task<IActionResult> Save([FromBody] MenuAppRoleViewModel model)
        {
            if (model != null)
            {
                var result = await _menuAppRoleApiClient.Save(new MenuAppRoleRequest()
                {
                    AppRoleId = model.AppRoleId,
                    MenuId = model.MenuId,
                    MenuAppRoleType = (int)model.MenuAppRoleType,


                });
                return Ok(result);
            }
            else
            {
                return Ok("Không có dữ liệu");
            }


        }
        [HttpGet()]
        public async Task<IActionResult> ListAllMenuAppRole(string appRoleId)
        {
            var request = new ManageMenuAppRolePagingRequest()
            {
                AppRoleId = appRoleId
            };
            var result = await _menuAppRoleApiClient.GetAll(request);
            return Ok(result);
        }
    }
}
