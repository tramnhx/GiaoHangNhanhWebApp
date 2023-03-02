using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
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
    public class KhuVucsController : ControllerBase
    {
        private readonly IKhuVucService _khuVucService;
        public KhuVucsController(IKhuVucService khuVucService)
        {
            _khuVucService = khuVucService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageKhuVucPagingRequest request)
        {
            var khuVucs = await _khuVucService.GetManageListPaging(request);
            return Ok(khuVucs);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManageKhuVucPagingRequest request)
        {
            var khuVucs = await _khuVucService.GetAll(request);
            return Ok(khuVucs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var khuVuc = await _khuVucService.GetById(id);
            if (khuVuc.IsSuccessed)
                if (khuVuc.ResultObj == null)
                    return BadRequest($"Không tìm thấy khu vực có id: {id}");
            return Ok(khuVuc);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] KhuVucRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.Id != null)
            {
                res = await _khuVucService.UpdateAsync(request);
            }
            else
            {
                res = await _khuVucService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _khuVucService.DeleteByIds(request);

            return Ok(result);
        }

    }
}
