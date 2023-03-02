using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
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
    public class BuuCucsController : ControllerBase
    {
        private readonly IBuuCucService _buuCucService;

        public BuuCucsController(IBuuCucService buuCucService)
        {
            _buuCucService = buuCucService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageBuuCucPagingRequest request)
        {
            var buuCucs = await _buuCucService.GetManageListPaging(request);
            return Ok(buuCucs);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManageBuuCucPagingRequest request)
        {
            var buuCucs = await _buuCucService.GetAll(request);
            return Ok(buuCucs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var buuCuc = await _buuCucService.GetById(id);
            if (buuCuc.IsSuccessed)
                if (buuCuc.ResultObj == null)
                    return BadRequest($"Không tìm thấy bưu cục có id: {id}");
            return Ok(buuCuc);
        }

        [HttpGet("GetByKhuVucId")]
        public async Task<IActionResult> GetByKhuVucId(int khuVucId)
        {
            var buuCuc = await _buuCucService.GetByKhuVucId(khuVucId);
            if (buuCuc.IsSuccessed)
                if (buuCuc.ResultObj == null)
                    return null;
            return Ok(buuCuc);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] BuuCucRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _buuCucService.UpdateAsync(request);
            }
            else
            {
                res = await _buuCucService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _buuCucService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
