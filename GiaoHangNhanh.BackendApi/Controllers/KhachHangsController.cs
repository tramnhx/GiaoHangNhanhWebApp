using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhachHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.Services.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangsController : ControllerBase
    {
        private readonly IKhachHangService _khachHangService;

        public KhachHangsController(IKhachHangService khachHangService)
        {
            _khachHangService = khachHangService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageKhachHangPagingRequest request)
        {
            var KhachHangs = await _khachHangService.GetManageListPaging(request);
            return Ok(KhachHangs);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManageKhachHangPagingRequest request)
        {
            var KhachHangs = await _khachHangService.GetAll(request);
            return Ok(KhachHangs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var KhachHang = await _khachHangService.GetById(id);
            if (KhachHang.IsSuccessed)
                if (KhachHang.ResultObj == null)
                    return BadRequest("Cannot find KhachHang");
            return Ok(KhachHang);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] KhachHangRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _khachHangService.UpdateAsync(request);
            }
            else
            {
                res = await _khachHangService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _khachHangService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
