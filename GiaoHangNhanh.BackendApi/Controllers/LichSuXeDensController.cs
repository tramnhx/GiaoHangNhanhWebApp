using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDens;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichSuXeDensController : ControllerBase
    {
        private readonly ILichSuXeDenService _lichSuXeDenService;

        public LichSuXeDensController(ILichSuXeDenService lichSuXeDenService)
        {
            _lichSuXeDenService = lichSuXeDenService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuXeDenPagingRequest request)
        {
            var lichSuXeDens = await _lichSuXeDenService.GetManageListPaging(request);
            return Ok(lichSuXeDens);
        }



        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] LichSuXeDenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _lichSuXeDenService.UpdateAsync(request);
            }
            else
            {
                res = await _lichSuXeDenService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuXeDenService.DeleteByIds(request);
            return Ok(result);
        }
    }
}

