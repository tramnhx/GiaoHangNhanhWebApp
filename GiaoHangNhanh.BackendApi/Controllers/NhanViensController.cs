using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens;
using GiaoHangNhanh.Services.System;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class NhanViensController : ControllerBase
    {
        private readonly INhanVienService _nhanVienService;
        public NhanViensController(INhanVienService nhanVienService)
        {
            _nhanVienService = nhanVienService;
        }
        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageNhanVienPagingRequest request)
        {
            var nhanViens = await _nhanVienService.GetManageListPaging(request);
            return Ok(nhanViens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gender = await _nhanVienService.GetById(id);
            if (gender.IsSuccessed)
                if (gender.ResultObj == null)
                    return BadRequest("Cannot find nhanvien");
            return Ok(gender);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] NhanVienRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.Id != null)
            {
                res = await _nhanVienService.UpdateAsync(request);
            }
            else
            {
                res = await _nhanVienService.CreateAsync(request);
            }

            return Ok(res);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] ManageNhanVienPagingRequest request)
        {
            var nhanViens = await _nhanVienService.GetAll(request);
            return Ok(nhanViens);
        }
        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _nhanVienService.DeleteByIds(request);

            return Ok(result);
        }
    }
}
