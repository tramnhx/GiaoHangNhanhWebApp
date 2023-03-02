using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.PhanLoaiVanDons;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhanLoaiVanDonsController : ControllerBase
    {
        private readonly IPhanLoaiVanDonService _phanLoaiHangBatThuongService;

        public PhanLoaiVanDonsController(IPhanLoaiVanDonService phanLoaiHangBatThuongService)
        {
            _phanLoaiHangBatThuongService = phanLoaiHangBatThuongService;
        }
        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManagePhanLoaiVanDonPagingRequest request)
        {
            var genders = await _phanLoaiHangBatThuongService.GetManageListPaging(request);
            return Ok(genders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var gender = await _phanLoaiHangBatThuongService.GetById(id);
            if (gender.IsSuccessed)
                if (gender.ResultObj == null)
                    return BadRequest("không tìm thấy phân loại hàng bất thường");
            return Ok(gender);
        }
        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] PhanLoaiVanDonRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res = null;
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
