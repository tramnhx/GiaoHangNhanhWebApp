using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuNhapKhos;
using GiaoHangNhanh.Services.Manipulation;
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
    public class LichSuNhapKhosController : ControllerBase
    {
        private readonly ILichSuNhapKhoService _lichSuNhapKhoService;
        public LichSuNhapKhosController(ILichSuNhapKhoService lichSuNhapKhoService)
        {
            _lichSuNhapKhoService = lichSuNhapKhoService;
        }
        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuNhapKhoPagingRequest request)
        {
            var lichsunhapkho = await _lichSuNhapKhoService.GetManageListPaging(request);
            return Ok(lichsunhapkho);
        }


        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] LichSuNhapKhoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.Id != null)
            {
                res = await _lichSuNhapKhoService.UpdateAsync(request);
            }
            else
            {
                res = await _lichSuNhapKhoService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuNhapKhoService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
