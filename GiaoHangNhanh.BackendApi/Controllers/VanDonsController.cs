using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
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
    public class VanDonsController : ControllerBase
    {
        private readonly IVanDonService _vanDonService;
        public VanDonsController(IVanDonService vanDonService)
        {
            _vanDonService = vanDonService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageVanDonPagingRequest request)
        {
            var vanDons = await _vanDonService.GetManageListPaging(request);
            return Ok(vanDons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int? id)
        {
            var vanDon = await _vanDonService.GetById(id);
            if (vanDon.IsSuccessed)
                if (vanDon.ResultObj == null)
                    return BadRequest($"Không tìm thấy vận đơn có id: {id}");
            return Ok(vanDon);
        }

        [HttpGet("GetByCode")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var vanDon = await _vanDonService.GetByCode(code);
            if (vanDon.IsSuccessed)
                if (vanDon.ResultObj == null)
                    return BadRequest($"Không tìm thấy vận đơn có mã: {code}");
            return Ok(vanDon);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] VanDonRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _vanDonService.UpdateAsync(request);
            }
            else
            {
                res = await _vanDonService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _vanDonService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
