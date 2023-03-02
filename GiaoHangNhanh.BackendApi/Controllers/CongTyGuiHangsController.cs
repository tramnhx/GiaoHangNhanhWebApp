using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
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
    public class CongTyGuiHangsController : ControllerBase
    {
        private readonly ICongTyGuiHangService _congTyGuiHangService;
        public CongTyGuiHangsController(ICongTyGuiHangService congTyGuiHangService)
        {
            _congTyGuiHangService = congTyGuiHangService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageCongTyGuiHangPagingRequest request)
        {
            var congTys = await _congTyGuiHangService.GetManageListPaging(request);
            return Ok(congTys);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManageCongTyGuiHangPagingRequest request)
        {
            var congTys = await _congTyGuiHangService.GetAll(request);
            return Ok(congTys);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var congTy = await _congTyGuiHangService.GetById(id);
            if (congTy.IsSuccessed)
                if (congTy.ResultObj == null)
                    return BadRequest($"Không tìm thấy công ty gửi hàng có id: {id}");
            return Ok(congTy);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] CongTyGuiHangRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _congTyGuiHangService.UpdateAsync(request);
            }
            else
            {
                res = await _congTyGuiHangService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _congTyGuiHangService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
