using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuDongBaos;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichSuDongBaosController : ControllerBase
    {
        private readonly ILichSuDongBaoService _LichSuDongBaoService;

        public LichSuDongBaosController(ILichSuDongBaoService LichSuDongBaoService)
        {
            _LichSuDongBaoService = LichSuDongBaoService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuDongBaoPagingRequest request)
        {
            var LichSuDongBaos = await _LichSuDongBaoService.GetManageListPaging(request);
            return Ok(LichSuDongBaos);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] LichSuDongBaoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _LichSuDongBaoService.UpdateAsync(request);
            }
            else
            {
                res = await _LichSuDongBaoService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _LichSuDongBaoService.DeleteByIds(request);
            return Ok(result);
        }
    }
}

