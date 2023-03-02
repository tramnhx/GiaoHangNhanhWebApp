using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
using GiaoHangNhanh.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MenusController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] ManageMenuPagingRequest request)
        {
            var menus = await _menuService.GetAll(request);
            return Ok(menus);
        }
        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageMenuPagingRequest request)
        {
            var result = await _menuService.GetManageListPaging(request);
            return Ok(result);
        }

        [HttpGet("gebyid")]
        public async Task<IActionResult> GetById([FromQuery] MenuRequest request)
        {
            var result = await _menuService.GetById(request);
            if (result.IsSuccessed)
            {
                if (result.ResultObj == null)
                {
                    return BadRequest("Cannot find menu");
                }
            }

            return Ok(result);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] MenuRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.Id != null)
            {
                res = await _menuService.UpdateAsync(request);
            }
            else
            {
                res = await _menuService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _menuService.DeleteByIds(request);

            return Ok(result);
        }
    }
}
