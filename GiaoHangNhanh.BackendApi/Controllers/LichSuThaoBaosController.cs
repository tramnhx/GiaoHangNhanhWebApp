using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuThaoBaos;
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
    public class LichSuThaoBaosController : ControllerBase
    {
        private readonly ILichSuThaoBaoService _LichSuThaoBaoService;

        public LichSuThaoBaosController(ILichSuThaoBaoService LichSuThaoBaoService)
        {
            _LichSuThaoBaoService = LichSuThaoBaoService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuThaoBaoPagingRequest request)
        {
            var LichSuThaoBaos = await _LichSuThaoBaoService.GetManageListPaging(request);
            return Ok(LichSuThaoBaos);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] LichSuThaoBaoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _LichSuThaoBaoService.UpdateAsync(request);
            }
            else
            {
                res = await _LichSuThaoBaoService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _LichSuThaoBaoService.DeleteByIds(request);
            return Ok(result);
        }
    }
}

