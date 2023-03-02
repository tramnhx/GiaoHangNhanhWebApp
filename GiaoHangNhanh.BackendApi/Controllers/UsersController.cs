
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UsersController(IUserService userService,
            IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authencate(model);

            if (string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("addorupdatestaffsecurity")]
        public async Task<IActionResult> AddOrUpdateStaffSecurity([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<string> res = await _userService.AddOrUpdateSecurityAsync(request);

            return Ok(res);
        }

        [HttpPost("addorupdatestaffprofiledetail")]
        public async Task<IActionResult> AddOrUpdateStaffProfileDetail([FromForm] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApiResult<string> res = await _userService.AddOrUpdateStaffProfileDetailAsync(request);

            return Ok(res);
        }

        [HttpGet("GetStaffManageListPaging")]
        public async Task<IActionResult> GetStaffManageListPaging([FromQuery] ManageUserPagingRequest request)
        {
            var users = await _userService.GetStaffManageListPaging(request);
            return Ok(users);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById([FromQuery] UserRequest request)
        {
            var user = await _userService.GetById(request);

            return Ok(user);
        }

        [HttpGet("getbyusername")]
        public async Task<IActionResult> GetByUserName([FromQuery] UserRequest request)
        {
            var user = await _userService.GetByUserName(request);

            return Ok(user);
        }

        [HttpPost()]
        public async Task<IActionResult> DeleteByIds(UserDeleteRequest request)
        {
            var result = await _userService.DeleteByIds(request);

            return Ok(result);
        }

        [HttpGet("getstaffprofiledetailbyuserid")]
        public async Task<IActionResult> GetStaffProfileDetailByUserId([FromQuery] UserRequest request)
        {
            var user = await _userService.GetStaffProfileDetailByUserId(request);

            return Ok(user);
        }

        [HttpGet("getstaffsecuritybyuserid")]
        public async Task<IActionResult> GetStaffSecurityByUserId([FromQuery] UserRequest request)
        {
            var user = await _userService.GetStaffSecurityByUserId(request);

            return Ok(user);
        }

        [HttpPost("deleteavatarbyuserid/{userId}")]
        public async Task<IActionResult> DeleteAvatarByUserId(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userService.DeleteAvatarByUserId(userId);

            return Ok(result);
        }

        [HttpGet("GetStaffFullNameListPaging")]
        public async Task<IActionResult> GetStaffFullNameListPaging([FromQuery] ManageUserPagingRequest request)
        {
            var users = await _userService.GetStaffFullNameListPaging(request);
            return Ok(users);
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var users = await _userService.ChangePasswordAsync(request);
            return Ok(users);
        }
    }
}
