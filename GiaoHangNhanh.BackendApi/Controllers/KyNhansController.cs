using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KyNhansController : ControllerBase
    {
        private readonly IKyNhanService _kyNhanService;
        public KyNhansController(IKyNhanService kyNhanService)
        {
            _kyNhanService = kyNhanService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageKyNhanPagingRequest request)
        {
            var kynhan = await _kyNhanService.GetManageListPaging(request);
            return Ok(kynhan);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var kynhan = await _kyNhanService.GetById(id);
            if (kynhan.IsSuccessed)
                if (kynhan.ResultObj == null)
                    return BadRequest("không tìm thấy người ký nhận này");
            return Ok(kynhan);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] KyNhanRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.Id != null)
            {
                res = await _kyNhanService.UpdateAsync(request);
            }
            else
            {
                res = await _kyNhanService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _kyNhanService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
