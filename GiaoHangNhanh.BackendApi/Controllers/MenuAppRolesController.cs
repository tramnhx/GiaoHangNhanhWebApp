using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles;
using GiaoHangNhanh.Services.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuAppRolesController : ControllerBase
    {
        private readonly IMenuAppRoleService _menuAppRoleService;
        public MenuAppRolesController(IMenuAppRoleService menuAppRoleService)
        {
            _menuAppRoleService = menuAppRoleService;
        }
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MenuAppRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            res = await _menuAppRoleService.Save(request);

            return Ok(res);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] ManageMenuAppRolePagingRequest request)
        {
            var stores = await _menuAppRoleService.GetAll(request);
            return Ok(stores);
        }
    }
}
