using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDis;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.PhanLoaiVanDons;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LichSuXeDisController : ControllerBase
    {
        private readonly ILichSuXeDiService _lichSuXeDiService;

        public LichSuXeDisController(ILichSuXeDiService lichSuXeDiService)
        {
            _lichSuXeDiService = lichSuXeDiService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageLichSuXeDiPagingRequest request)
        {
            var lichSuXeDis = await _lichSuXeDiService.GetManageListPaging(request);
            return Ok(lichSuXeDis);
        }

        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAll([FromQuery] ManageLichSuXeDiPagingRequest request)
        //{
        //    var lichSuXeDis = await _lichSuXeDiService./*GetAll*/(request);
        //    return Ok(lichSuXeDis);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(int id)
        //{
        //    var lichSuXeDen = await _lichSuXeDiService.GetById(id);
        //    if (lichSuXeDen.IsSuccessed)
        //        if (lichSuXeDen.ResultObj == null)
        //            return BadRequest($"Không tìm thấy xe đến có id: {id}");
        //    return Ok(lichSuXeDen);
        //}

        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] LichSuXeDiRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _lichSuXeDiService.UpdateAsync(request);
            }
            else
            {
                res = await _lichSuXeDiService.CreateAsync(request);
            }

            return Ok(res);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(DeleteRequest request)
        {
            var result = await _lichSuXeDiService.DeleteByIds(request);
            return Ok(result);
        }
    }
}