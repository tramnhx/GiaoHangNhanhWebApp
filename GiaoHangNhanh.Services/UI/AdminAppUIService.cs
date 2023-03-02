using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
using GiaoHangNhanh.DAL.Entities.EntityDto.UI.AdminApp;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.Services.UI
{
    public interface IAdminAppUIService
    {
        Task<ApiResult<AdminAppHeaderDto>> GetHeaderObjects(AdminAppHeaderRequest request);
        ApiResult<AdminAppLoginDto> GetLoginObjects();
        Task<ApiResult<AdminAppLeftSideBarDto>> GetLeftSideBarObjects();
    }

    public class AdminAppUIService : IAdminAppUIService
    {
        private readonly IConfiguration _configuration;
        private readonly GiaoHangNhanhDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminAppUIService(GiaoHangNhanhDbContext context, IConfiguration configuration,
                                UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApiResult<AdminAppHeaderDto>> GetHeaderObjects(AdminAppHeaderRequest request)
        {
            var adminAppHeaderViewModel = new AdminAppHeaderDto();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
            {
                adminAppHeaderViewModel.Email = user.Email;
                adminAppHeaderViewModel.FullName = $"{user.FirstName} {user.LastName}";
                adminAppHeaderViewModel.UserImage = (!string.IsNullOrEmpty(user.Avatar) ? (_configuration[SystemConstants.UserConstants.UserImagePath] + "/" + user.Avatar) : _configuration[SystemConstants.AppConstants.FileNoImagePerson]);
            }

            return new ApiSuccessResult<AdminAppHeaderDto>(adminAppHeaderViewModel);
        }
        public ApiResult<AdminAppLoginDto> GetLoginObjects()
        {
            var adminAppLoginViewModel = new AdminAppLoginDto()
            {
                Logo = _configuration[SystemConstants.AppConstants.LogoFile2]
            };

            return new ApiSuccessResult<AdminAppLoginDto>(adminAppLoginViewModel);
        }
        public async Task<ApiResult<AdminAppLeftSideBarDto>> GetLeftSideBarObjects()
        {
            var adminAppLeftSideBarViewModel = new AdminAppLeftSideBarDto()
            {
                Logo = _configuration[SystemConstants.AppConstants.LogoFile2],
                Menus = await (from m in _context.Menus
                               where m.IsActive == true && m.IsDeleted == false
                               select new MenuDto()
                               {
                                   Id = m.Id,
                                   SortOrder = m.SortOrder,
                                   Code = m.Code,
                                   Name = m.Name,
                                   ParentId = m.ParentId,
                                   Link = m.Link,
                                   Icon = m.Icon
                               }).AsNoTracking().ToListAsync()
            };

            return new ApiSuccessResult<AdminAppLeftSideBarDto>(adminAppLeftSideBarViewModel);
        }
    }
}
