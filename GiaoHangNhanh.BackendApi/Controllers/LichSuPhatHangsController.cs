using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuPhatHangs;
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
    public class LichSuPhatHangsController : ControllerBase
    {
        private readonly ILichSuPhatHangService _lichSuPhatHangService;
        public LichSuPhatHangsController(ILichSuPhatHangService lichSuPhatHangService)
        {
            _lichSuPhatHangService = lichSuPhatHangService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuPhatHangPagingRequest request)
        {
            var lichSuPhatHangs = await _lichSuPhatHangService.GetManageListPaging(request);
            return Ok(lichSuPhatHangs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            var lichSuPhatHang = await _lichSuPhatHangService.GetById(id);
            if (lichSuPhatHang.IsSuccessed)
                if (lichSuPhatHang.ResultObj == null)
                    return BadRequest($"Không tìm thấy vận đơn có id: {id}");
            return Ok(lichSuPhatHang);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] LichSuPhatHangRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _lichSuPhatHangService.UpdateAsync(request);
            }
            else
            {
                res = await _lichSuPhatHangService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuPhatHangService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
