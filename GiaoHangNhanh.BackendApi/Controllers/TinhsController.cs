using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
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
    public class TinhsController : ControllerBase
    {
        private readonly ITinhService _tinhService;

        public TinhsController(ITinhService tinhService)
        {
            _tinhService = tinhService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageTinhPagingRequest request)
        {
            var tinhs = await _tinhService.GetManageListPaging(request);
            return Ok(tinhs);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManageTinhPagingRequest request)
        {
            var tinhs = await _tinhService.GetAll(request);
            return Ok(tinhs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tinh = await _tinhService.GetById(id);
            if (tinh.IsSuccessed)
                if (tinh.ResultObj == null)
                    return BadRequest("Cannot find DMTInh");
            return Ok(tinh);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] TinhRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _tinhService.UpdateAsync(request);
            }
            else
            {
                res = await _tinhService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _tinhService.DeleteByIds(request);
            return Ok(result);
        }


    }
}
