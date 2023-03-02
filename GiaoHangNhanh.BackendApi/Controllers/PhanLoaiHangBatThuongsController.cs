using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhanLoaiHangBatThuongs;
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
    public class PhanLoaiHangBatThuongsController : ControllerBase
    {
        private readonly IPhanLoaiHangBatThuongService _phanLoaiHangBatThuongService;

        public PhanLoaiHangBatThuongsController(IPhanLoaiHangBatThuongService phanLoaiHangBatThuongService)
        {
            _phanLoaiHangBatThuongService = phanLoaiHangBatThuongService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManagePhanLoaiHangBatThuongPagingRequest request)
        {
            var phanLoaiHangBatThuongs = await _phanLoaiHangBatThuongService.GetManageListPaging(request);
            return Ok(phanLoaiHangBatThuongs);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] ManagePhanLoaiHangBatThuongPagingRequest request)
        {
            var phanLoaiHangBatThuongs = await _phanLoaiHangBatThuongService.GetAll(request);
            return Ok(phanLoaiHangBatThuongs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var phanLoaiHangBatThuong = await _phanLoaiHangBatThuongService.GetById(id);
            if (phanLoaiHangBatThuong.IsSuccessed)
                if (phanLoaiHangBatThuong.ResultObj == null)
                    return BadRequest($"Không tìm thấy phân loại hàng bất thường có id: {id}");
            return Ok(phanLoaiHangBatThuong);
        }

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] PhanLoaiHangBatThuongRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _phanLoaiHangBatThuongService.UpdateAsync(request);
            }
            else
            {
                res = await _phanLoaiHangBatThuongService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _phanLoaiHangBatThuongService.DeleteByIds(request);
            return Ok(result);
        }
    }
}
