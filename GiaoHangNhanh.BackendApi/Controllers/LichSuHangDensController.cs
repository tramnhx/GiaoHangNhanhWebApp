using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuHangDens;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichSuHangDensController : ControllerBase
    {
        private readonly ILichSuHangDenService _lichSuHangDenService;
        public LichSuHangDensController(ILichSuHangDenService lichSuHangDenService)
        {
            _lichSuHangDenService = lichSuHangDenService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] LichSuHangDenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.VanDonIds != null)
            {
                foreach (var vanDon in request.VanDonIds)
                {
                    res = await _lichSuHangDenService.CreateWithVanDonAsync(request, vanDon);
                }
            }    
            return Ok(res);
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuHangDenPagingRequest request)
        {
            var hangDens = await _lichSuHangDenService.GetManageListPaging(request);
            return Ok(hangDens);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuHangDenService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
