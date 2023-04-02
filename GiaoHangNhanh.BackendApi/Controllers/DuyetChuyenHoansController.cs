using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DuyetChuyenHoans;
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
    public class DuyetChuyenHoansController : Controller
    {
       
        private readonly IDuyetChuyenHoanService _duyetChuyenHoanService;
        public DuyetChuyenHoansController(IDuyetChuyenHoanService DuyetChuyenHoanService)
        {
            _duyetChuyenHoanService = DuyetChuyenHoanService;
        }

        [HttpGet("GetManageListPaging")]
        public async Task<IActionResult> GetManageListPaging([FromQuery] ManageDuyetChuyenHoanPagingRequest request)
        {
            var congTys = await _duyetChuyenHoanService.GetManageListPaging(request);
            return Ok(congTys);
        }


        [HttpPost("addorupdate")]
        public async Task<IActionResult> AddOrUpdate([FromBody] DuyetChuyenHoanRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<int> res;

            if (request.Id != null)
            {
                res = await _duyetChuyenHoanService.UpdateAsync(request);
            }
            else
            {
                res = await _duyetChuyenHoanService.CreateAsync(request);
            }

            return Ok(res);
        }

    }
    }
