using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles;
using GiaoHangNhanh.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppRolesController : ControllerBase
    {
        private readonly IAppRoleService _appRoleService;

        public AppRolesController(IAppRoleService appRoleService)
        {
            _appRoleService = appRoleService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] ManageAppRolePagingRequest request)
        {
            var appRoles = await _appRoleService.GetAll(request);
            return Ok(appRoles);
        }
        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageAppRolePagingRequest request)
        {
            var appRoles = await _appRoleService.GetManageListPaging(request);
            return Ok(appRoles);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var appRole = await _appRoleService.GetById(id);
            if (appRole.IsSuccessed)
            {
                if (appRole.ResultObj == null)
                {
                    return BadRequest("Cannot find appRole");
                }
            }

            return Ok(appRole);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] AppRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<string> res = null;
            if (!string.IsNullOrEmpty(request.Id))
            {
                res = await _appRoleService.UpdateAsync(request);

            }
            else
            {
                res = await _appRoleService.CreateAsync(request);
            }

            return Ok(res);
        }


        [HttpPost("deletebyids")]
        public async Task<IActionResult> DeleteByIds(AppRoleDeleteRequest request)
        {
            var result = await _appRoleService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
