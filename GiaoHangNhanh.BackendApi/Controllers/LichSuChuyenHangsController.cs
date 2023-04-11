using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuChuyenHangs;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichSuChuyenHangsController : ControllerBase
    {
        private readonly ILichSuChuyenHangService _lichSuChuyenHangService;
        public LichSuChuyenHangsController(ILichSuChuyenHangService lichSuChuyenHangService)
        {
            _lichSuChuyenHangService = lichSuChuyenHangService;
        }

        [HttpPost("AddXeDiOrXeDen")]
        public async Task<IActionResult> AddXeDiOrXeDen([FromBody] LichSuChuyenHangRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;

            if (request.SealXes != null)
            {
                foreach (var item in request.SealXes)
                {
                    if (request.Id != null)
                    {
                        res = await _lichSuChuyenHangService.UpdateAsync(request);
                    }
                    else
                    {
                        res = await _lichSuChuyenHangService.CreateXeDenXeDiAsync(request, item);
                    }
                }
            }

            return Ok(res);
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuChuyenHangPagingRequest request)
        {
            var chuyenHangs = await _lichSuChuyenHangService.GetManageListPaging(request);
            return Ok(chuyenHangs);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuChuyenHangService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
