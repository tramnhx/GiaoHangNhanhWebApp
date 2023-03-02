using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans;
using GiaoHangNhanh.Services.Manipulation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DangKyChuyenHoansController : Controller
    {
       
            private readonly IDangKyChuyenHoanService _dangkychuyenhoanService;
            public DangKyChuyenHoansController(IDangKyChuyenHoanService DangKyChuyenHoanService)
            {
                _dangkychuyenhoanService = DangKyChuyenHoanService;
            }

            [HttpGet("GetManageListPaging")]
            public async Task<IActionResult> GetManageListPaging([FromQuery] ManageDangKyChuyenHoanPagingRequest request)
            {
                var congTys = await _dangkychuyenhoanService.GetManageListPaging(request);
                return Ok(congTys);
            }

 
            [HttpPost("addorupdate")]
            public async Task<IActionResult> AddOrUpdate([FromBody] DangKyChuyenHoanRequest request)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                ApiResult<int> res;

                if (request.Id != null)
                {
                    res = await _dangkychuyenhoanService.UpdateAsync(request);
                }
                else
                {
                    res = await _dangkychuyenhoanService.CreateAsync(request);
                }

                return Ok(res);
            }

            [HttpPost()]
            public async Task<IActionResult> DeleteByIds(DeleteRequest request)
            {
                var result = await _dangkychuyenhoanService.DeleteByIds(request);
                return Ok(result);
            }
        }
    }
