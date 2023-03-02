using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.Services.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DichVusController : ControllerBase
    {
        private readonly IDichVuService _dichVuService;

        public DichVusController(IDichVuService dichVuService)
        {
            _dichVuService = dichVuService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageDichVuPagingRequest request)
        {
            var dichVus = await _dichVuService.GetManageListPaging(request);
            return Ok(dichVus);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManageDichVuPagingRequest request)
        {
            var dichVus = await _dichVuService.GetAll(request);
            return Ok(dichVus);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dichVu = await _dichVuService.GetById(id);
            if (dichVu.IsSuccessed)
                if (dichVu.ResultObj == null)
                    return BadRequest($"Không tìm thấy dịch vụ có id: {id}");
            return Ok(dichVu);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] DichVuRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _dichVuService.UpdateAsync(request);
            }
            else
            {
                res = await _dichVuService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _dichVuService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
