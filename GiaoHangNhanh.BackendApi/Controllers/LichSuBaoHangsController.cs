using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs;
using GiaoHangNhanh.Services.Manipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichSuBaoHangsController : ControllerBase
    {
        private readonly ILichSuBaoHangService _lichSuBaoHangService;
        public LichSuBaoHangsController(ILichSuBaoHangService lichSuBaoHangService)
        {
            _lichSuBaoHangService = lichSuBaoHangService;
        }
        [HttpGet("GetManageBaoHangListPaging")]
        public async Task<IActionResult> GetManageBaoHangListPaging([FromQuery] ManageLichSuBaoHangPagingRequest request)
        {
            var huyens = await _lichSuBaoHangService.GetManageBaoHangListPaging(request);
            return Ok(huyens);
        }
        [HttpGet("GetManageVanDonInBaoListPaging")]
        public async Task<IActionResult> GetManageVanDonInBaoListPaging([FromQuery] ManageLichSuBaoHangPagingRequest request)
        {
            var huyens = await _lichSuBaoHangService.GetManageVanDonInBaoListPaging(request);
            return Ok(huyens);
        }

        [HttpPost("layhangrabao")]
        public async Task<IActionResult> LayHangRaBao(DeleteRequest request)
        {
            var result = await _lichSuBaoHangService.LayHangRaBao(request);
            return Ok(result);
        }
        [HttpGet("{baoHangId}")]
        public async Task<IActionResult> GetById(int baoHangId)
        {
            var baoHang = await _lichSuBaoHangService.GetById(baoHangId);
            if (baoHang.IsSuccessed)
                if (baoHang.ResultObj == null)
                    return BadRequest("Cannot find bao hang");
            return Ok(baoHang);
        }

        [HttpPost("dongbaohang")]
        public async Task<IActionResult> DongBaoHang([FromBody] LichSuBaoHangRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            res = await _lichSuBaoHangService.DongBaoHangAsync(request);

            return Ok(res);
        }
        [HttpPost("thaobaohang")]
        public async Task<IActionResult> ThaoBaoHang([FromBody] LichSuBaoHangRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            res = await _lichSuBaoHangService.ThaoBaoHangAsync(request);

            return Ok(res);
        }
        [HttpPost("addvandoninbao")]
        public async Task<IActionResult> AddVanDonInBao([FromBody] VanDonInBaoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.VanDonIds != null)
            {
                res = await _lichSuBaoHangService.CreateVanDonInBaoHangAsync(request);
            }

            return Ok(res);
        }
        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuBaoHangService.DeleteByIds(request);
            return Ok(result);
        }
        [HttpPost("deletebaohangbyids")]
        public async Task<IActionResult> DeleteBaoHangByIds(DeleteRequest request)
        {
            var result = await _lichSuBaoHangService.DeleteBaoHangIds(request);
            return Ok(result);
        }

    }
}
