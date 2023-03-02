
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhuongThucThanhToans;
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
    public class PhuongThucThanhToansController : ControllerBase
    {
        private readonly IPhuongThucThanhToanService _phuongThucThanhToanService;
        public PhuongThucThanhToansController(IPhuongThucThanhToanService phuongThucThanhToanService)
        {
            _phuongThucThanhToanService = phuongThucThanhToanService;
        }
        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManagePhuongThucThanhToanPagingRequest request)
        {
            var phuongThucThanhToans = await _phuongThucThanhToanService.GetManageListPaging(request);
            return Ok(phuongThucThanhToans);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManagePhuongThucThanhToanPagingRequest request)
        {
            var phuongThucThanhToans = await _phuongThucThanhToanService.GetAll(request);
            return Ok(phuongThucThanhToans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var phuongThucThanhToan = await _phuongThucThanhToanService.GetById(id);
            if (phuongThucThanhToan.IsSuccessed)
                if (phuongThucThanhToan.ResultObj == null)
                    return BadRequest($"Không tìm thấy phương thức thanh toán có id: {id}");
            return Ok(phuongThucThanhToan);
        }
        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] PhuongThucThanhToanRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
            if (request.Id != null)
            {
                res = await _phuongThucThanhToanService.UpdateAsync(request);
            }
            else
            {
                res = await _phuongThucThanhToanService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _phuongThucThanhToanService.DeleteByIds(request);

            return Ok(result);
        }
    }
}
