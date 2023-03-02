using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
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
    public class HuyensController : ControllerBase
    {
        private readonly IHuyenService _huyenService;
        public HuyensController(IHuyenService huyenService)
        {
            _huyenService = huyenService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManageHuyenPagingRequest request)
        {
            var dichVus = await _huyenService.GetAll(request);
            return Ok(dichVus);
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageHuyenPagingRequest request)
        {
            var huyens = await _huyenService.GetManageListPaging(request);
            return Ok(huyens);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var huyen = await _huyenService.GetById(id);
            if (huyen.IsSuccessed)
                if (huyen.ResultObj == null)
                    return BadRequest("Cannot find huyen");
            return Ok(huyen);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] HuyenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.Id != null)
            {
                res = await _huyenService.UpdateAsync(request);
            }
            else
            {
                res = await _huyenService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _huyenService.DeleteByIds(request);

            return Ok(result);
        }
    }
}
