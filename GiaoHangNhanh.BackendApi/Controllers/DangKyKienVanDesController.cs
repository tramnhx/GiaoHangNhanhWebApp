using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyKienVanDes;
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
    public class DangKyKienVanDesController : ControllerBase
    {
        private readonly IDangKyKienVanDeService _dangKyKienVanDeService;
        public DangKyKienVanDesController(IDangKyKienVanDeService dangKyKienVanDeService)
        {
            _dangKyKienVanDeService = dangKyKienVanDeService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageDangKyKienVanDePagingRequest request)
        {
            var dangKyKienVanDes = await _dangKyKienVanDeService.GetManageListPaging(request);
            return Ok(dangKyKienVanDes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            var dangKyKienVanDe = await _dangKyKienVanDeService.GetById(id);
            if (dangKyKienVanDe.IsSuccessed)
                if (dangKyKienVanDe.ResultObj == null)
                    return BadRequest($"Không tìm thấy vận đơn có id: {id}");
            return Ok(dangKyKienVanDe);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] DangKyKienVanDeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _dangKyKienVanDeService.UpdateAsync(request);
            }
            else
            {
                res = await _dangKyKienVanDeService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _dangKyKienVanDeService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
