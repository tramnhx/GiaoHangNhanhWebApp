using GiaoHangNhanh.DAL.Entities.EntityDto.UI.AdminApp;
using GiaoHangNhanh.Services.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GiaoHangNhanh.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminAppUIsController : ControllerBase
    {
        private readonly IAdminAppUIService _adminAppUIService;

        public AdminAppUIsController(IAdminAppUIService adminAppUIService)
        {
            _adminAppUIService = adminAppUIService;
        }
        [HttpGet("getHeaderObjects")]
        public async Task<IActionResult> GetHeaderObjects(string userName)
        {
            var data = await _adminAppUIService.GetHeaderObjects(new AdminAppHeaderRequest() { UserName = userName });
            return Ok(data);
        }
        [HttpGet("getLoginObjects")]
        [AllowAnonymous]
        public IActionResult GetLoginObjects()
        {
            var data = _adminAppUIService.GetLoginObjects();
            return Ok(data);
        }
        [HttpGet("getLeftSideBarObjects")]
        public async Task<IActionResult> GetLeftSideBarObjects()
        {
            var data = await _adminAppUIService.GetLeftSideBarObjects();
            return Ok(data);
        }
    }
}
