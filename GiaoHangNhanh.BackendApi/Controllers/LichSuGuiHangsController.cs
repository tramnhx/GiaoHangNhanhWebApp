using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuGuiHangs;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichSuGuiHangsController : ControllerBase
    {
        private readonly ILichSuGuiHangService _lichSuGuiHangService;

        public LichSuGuiHangsController(ILichSuGuiHangService lichSuGuiHangService)
        {
            _lichSuGuiHangService = lichSuGuiHangService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuGuiHangPagingRequest request)
        {
            var lichSuGuiHangs = await _lichSuGuiHangService.GetManageListPaging(request);
            return Ok(lichSuGuiHangs);
        }



        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] LichSuGuiHangRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _lichSuGuiHangService.UpdateAsync(request);
            }
            else
            {
                res = await _lichSuGuiHangService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuGuiHangService.DeleteByIds(request);
            return Ok(result);
        }
    }
}

