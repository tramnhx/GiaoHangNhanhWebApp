using GiaoHangNhanh.DAL.EF;
using GiaoHangNhanh.DAL.Entities.Entity;
using GiaoHangNhanh.Utilities.Constants;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles;
using GiaoHangNhanh.Services.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IO;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Net.Http.Headers;

namespace GiaoHangNhanh.Services.System
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);
        Task<ApiResult<int>> ChangePasswordAsync(ChangePasswordRequest request);
        Task<PagedResult<UserDto>> GetStaffManageListPaging(ManageUserPagingRequest request);
        Task<ApiResult<UserDto>> GetById(UserRequest request);
        Task<ApiResult<UserDto>> GetByUserName(UserRequest request);
        Task<ApiResult<int>> DeleteByIds(UserDeleteRequest request);
        Task<ApiResult<UserDto>> GetStaffProfileDetailByUserId(UserRequest request);
        Task<ApiResult<UserDto>> GetStaffSecurityByUserId(UserRequest request);
        Task<ApiResult<string>> AddOrUpdateStaffProfileDetailAsync(UserRequest request);
        Task<ApiResult<string>> AddOrUpdateSecurityAsync(UserRequest request);
        Task<ApiResult<string>> DeleteAvatarByUserId(string userId);
        Task<PagedResult<UserDto>> GetStaffFullNameListPaging(ManageUserPagingRequest request);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly GiaoHangNhanhDbContext _context;
        private readonly IFileStorageService _storageService;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration,
            IFileStorageService storageService,
            GiaoHangNhanhDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _storageService = storageService;
            _context = context;
        }

        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);
                if (user == null)
                {
                    return new ApiErrorResult<string>("Tài khoản không tồn tại");
                }
                else if (!user.IsActive)
                {
                    return new ApiErrorResult<string>("Tài khoản chưa được kích hoạt");
                }

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
                if (!result.Succeeded)
                {
                    return new ApiErrorResult<string>("Mật khẩu không đúng");
                }

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                    new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role, string.Join(";",roles)),
                    new Claim(ClaimTypes.Name, request.UserName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_configuration["Tokens:Issuer"],
                    _configuration["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds);

                return new ApiSuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>(ex.Message);
            }
        }
        public async Task<ApiResult<UserDto>> GetById(UserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                return new ApiErrorResult<UserDto>("User không tồn tại");
            }

            var userDto = new UserDto()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Dob = user.Dob,
                Id = user.Id.ToString(),
                MaNhanVien = user.MaNhanVien,
                LastName = user.LastName,
                UserName = user.UserName,
                Avatar = (!string.IsNullOrEmpty(user.Avatar) ? _configuration[SystemConstants.UserConstants.UserImagePath] + "/" + user.Avatar : _configuration[SystemConstants.AppConstants.FileNoImagePerson]),
                IsActive = user.IsActive
            };
            return new ApiSuccessResult<UserDto>(userDto);
        }
        public async Task<ApiResult<UserDto>> GetByUserName(UserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ApiErrorResult<UserDto>("User không tồn tại");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserDto()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                Dob = user.Dob,
                MaNhanVien = user.MaNhanVien,
                Id = user.Id.ToString(),
                LastName = user.LastName,
                UserName = user.UserName,
                Avatar = (!string.IsNullOrEmpty(user.Avatar) ? (_configuration[SystemConstants.UserConstants.UserImagePath] + "/" + user.Avatar) : SystemConstants.AppConstants.FileNoImagePerson),
                IsActive = user.IsActive
            };
            return new ApiSuccessResult<UserDto>(userViewModel);
        }
        public async Task<PagedResult<UserDto>> GetStaffManageListPaging(ManageUserPagingRequest request)
        {
            try
            {
                var query = from u in _context.Users
                            select new { u };

                if (!string.IsNullOrEmpty(request.TextSearch))
                {
                    query = query.Where(x => x.u.UserName.Contains(request.TextSearch)
                     || (x.u.LastName + " " + x.u.FirstName).Contains(request.TextSearch)
                     || (x.u.Email).Contains(request.TextSearch)
                     );
                }

                //3. Paging
                int totalRow = await query.CountAsync();

                if (request.PageIndex != null)
                {
                    query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                                    .Take(request.PageSize);
                }

                var data = await query.Select(x => new UserDto()
                {
                    Id = x.u.Id.ToString(),
                    Id_Image_FullName_Email = x.u.Id.ToString() + "|" + (!string.IsNullOrEmpty(x.u.Avatar) ? (_configuration[SystemConstants.UserConstants.UserImagePath] + "/" + x.u.Avatar) : _configuration[SystemConstants.AppConstants.FileNoImagePerson]) + "|" +
                                            x.u.LastName + x.u.FirstName + "|" + x.u.Email,
                    UserName = x.u.UserName,
                    MaNhanVien = x.u.MaNhanVien,
                    PhoneNumber = x.u.PhoneNumber,
                    Address = x.u.Address,
                    StrCreatedDate = x.u.CreatedDate.ToString("dd/MM/yyyy")
                }).AsNoTracking().ToListAsync();

                List<AppRoleDto> appRoles = await (from ur in _context.UserRoles
                                                   join r in _context.Roles on ur.RoleId equals r.Id
                                                   where data.Select(m => Guid.Parse(m.Id)).ToList().Contains(ur.UserId)
                                                   select new AppRoleDto()
                                                   {
                                                       UserId = ur.UserId,
                                                       Name = r.Name
                                                   }).ToListAsync();
                foreach (var user in data)
                {
                    user.AppRoles = appRoles.Where(m => m.UserId == Guid.Parse(user.Id)).ToList();
                }

                //4. Select and projection
                var pagedResult = new PagedResult<UserDto>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
                return pagedResult;
            }
            catch (Exception ex)
            {
                return new PagedResult<UserDto>()
                {
                    TotalRecords = 0,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = null,
                    Message = ex.Message
                };
            }

        }
        public async Task<ApiResult<string>> AddOrUpdateSecurityAsync(UserRequest request)
        {
            try
            {
                bool isNew = false;
                AppUser user = null;

                if (string.IsNullOrEmpty(request.Id))
                {
                    isNew = true;

                    user = await _userManager.FindByNameAsync(request.UserName);
                    if (user != null)
                    {
                        return new ApiErrorResult<string>("Tài khoản đã tồn tại");
                    }
                    else if (await _userManager.FindByEmailAsync(request.Email) != null)
                    {
                        return new ApiErrorResult<string>("Emai đã tồn tại");
                    }

                    user = new AppUser()
                    {
                        CreatedDate = DateTime.Now,
                        MaNhanVien = $"NV{DateTime.Now.Day}{DateTime.Now.Month}{DateTime.Now.Year}{DateTime.Now.Hour}{DateTime.Now.Minute}",
                    };
                }
                else
                {
                    user = await _userManager.FindByIdAsync(request.Id.ToString());
                    if (await _userManager.Users.AnyAsync(x => x.Email == request.Email && x.Id != Guid.Parse(request.Id)))
                    {
                        return new ApiErrorResult<string>("Emai đã tồn tại");
                    }
                }

                user.UserName = request.UserName;
                user.Email = request.Email;
                user.ModifiedUserId = Guid.Parse(request.ModifiedUserId);
                user.IsActive = request.IsActive;
                if (request.IsActive == true)
                {
                    user.ActivateDate = DateTime.Now;
                }

                user.ModifiedDate = DateTime.Now;

                if (isNew == true)
                {
                    var result = await _userManager.CreateAsync(user, request.Password);
                    if (!result.Succeeded)
                    {
                        return new ApiErrorResult<string>("Đăng ký không thành công");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(request.Password))
                    {
                        if (user.PasswordHash != null)
                        {
                            if (!(await _userManager.RemovePasswordAsync(user)).Succeeded)
                            {
                                return new ApiErrorResult<string>("Xóa mật khẩu cũ không thành công.");
                            }
                        }

                        if (!(await _userManager.AddPasswordAsync(user, request.Password)).Succeeded)
                        {
                            return new ApiErrorResult<string>("Đổi mật khẩu không thành công.");
                        }
                    }

                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        return new ApiErrorResult<string>("Cập nhật không thành công");
                    }
                }

                await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                if (request.AppRoleCodes != null)
                {
                    foreach (string roleCode in request.AppRoleCodes)
                    {
                        var a = roleCode;
                        await _userManager.AddToRoleAsync(user, roleCode);
                    }
                }

                return new ApiSuccessResult<string>(user.Id.ToString());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>(ex.Message);
            }
        }
        public async Task<ApiResult<string>> AddOrUpdateStaffProfileDetailAsync(UserRequest request)
        {
            try
            {
                DateTime ngayValue;
                var user = await _userManager.FindByIdAsync(request.Id);
                if (user == null)
                {
                    return new ApiErrorResult<string>("User này tồn tại");
                }

                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.MaNhanVien = request.MaNhanVien;
                user.Dob = (DateTime.TryParseExact(request.Dob, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>());
                user.PhoneNumber = request.PhoneNumber;
                user.Description = request.Description;
                user.ModifiedUserId = Guid.Parse(request.ModifiedUserId);
                user.Address = request.Address;
                user.StartingDate = (DateTime.TryParseExact(request.StartingDate, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : DateTime.Now);
                if (!string.IsNullOrEmpty(request.LeaveDate))
                {
                    user.LeaveDate = (DateTime.TryParseExact(request.LeaveDate, _configuration[SystemConstants.AppConstants.DateFormat], null, DateTimeStyles.None, out ngayValue) ? ngayValue : new Nullable<DateTime>());
                }


                if (request.Avatar != null)
                {
                    if (!string.IsNullOrEmpty(user.Avatar))
                    {
                        await DeleteFile(user.Avatar);
                    }

                    user.Avatar = await this.SaveFile(request.Avatar);
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new ApiSuccessResult<string>(user.Id.ToString());
                }

                return new ApiErrorResult<string>("Cập nhật không thành công");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>(ex.Message);
            }
        }
        public async Task<ApiResult<int>> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return new ApiErrorResult<int>("Không tìm thấy nhân viên này");
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (result.Succeeded)
            {
                return new ApiSuccessResult<int>();
            }

            return new ApiErrorResult<int>("Cập nhật không thành công");
        }
        private async Task<ApiResult<int>> DeleteById(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return new ApiErrorResult<int>("User không tồn tại");
                }

                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    await this.DeleteFile(user.Avatar);
                }

                var userResult = await _userManager.DeleteAsync(user);

                if (userResult.Succeeded)
                {
                    return new ApiSuccessResult<int>(1);
                }

                return new ApiErrorResult<int>("Xóa UserId: " + id.ToString() + " không thành công");

            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>("UserId: " + id.ToString() + ": " + ex.Message);
            }
        }
        public async Task<ApiResult<int>> DeleteByIds(UserDeleteRequest request)
        {
            var ids = request.Ids.Select(m => Guid.Parse(m)).ToList();
            bool check = true;
            int n = 0;
            while (check == true && n < ids.Count)
            {
                var result = await DeleteById(ids[n]);
                if (result.IsSuccessed)
                {
                    n++;
                }
                else
                {
                    return result;
                }
            }

            return new ApiSuccessResult<int>();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            try
            {
                await _storageService.SaveFileAsync(file.OpenReadStream(), _configuration[SystemConstants.UserConstants.UserImagePath] + "/" + fileName);
                return fileName;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        private async Task DeleteFile(string fileName)
        {
            await _storageService.DeleteFileAsync(_configuration[SystemConstants.UserConstants.UserImagePath] + "/" + fileName);
        }
        public async Task<ApiResult<UserDto>> GetStaffProfileDetailByUserId(UserRequest request)
        {
            try
            {
                var user = await (from u in _userManager.Users
                                  where u.Id == Guid.Parse(request.Id)
                                  select new UserDto()
                                  {
                                      Id = u.Id.ToString(),
                                      MaNhanVien = u.MaNhanVien,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      Dob = u.Dob,
                                      Address = u.Address,
                                      PhoneNumber = u.PhoneNumber,
                                      StrStartingDate = u.StartingDate.ToString("dd/MM/yyyy"),
                                      StrLeaveDate = u.LeaveDate != null ? u.LeaveDate.Value.ToString("dd/MM/yyyy") : string.Empty,
                                      Avatar = (!string.IsNullOrEmpty(u.Avatar) ? _configuration[SystemConstants.UserConstants.UserImagePath] + "/" + u.Avatar : _configuration[SystemConstants.AppConstants.FileNoImagePerson])
                                  }
                            ).AsNoTracking().FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ApiErrorResult<UserDto>("User không tồn tại.");
                }

                return new ApiSuccessResult<UserDto>(user);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<UserDto>(ex.Message);
            }
        }
        public async Task<ApiResult<UserDto>> GetStaffSecurityByUserId(UserRequest request)
        {
            try
            {
                var user = await (from u in _userManager.Users
                                  where u.Id == Guid.Parse(request.Id)
                                  select new UserDto()
                                  {
                                      Id = u.Id.ToString(),
                                      Code = u.Code,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      Dob = u.Dob,
                                      MaNhanVien = u.MaNhanVien,
                                      PhoneNumber = u.PhoneNumber,
                                      Avatar = (u.Avatar != null ? _configuration[SystemConstants.UserConstants.UserImagePath] + "/" + u.Avatar : _configuration[SystemConstants.AppConstants.FileNoImagePerson]),
                                      UserName = u.UserName,
                                      Email = u.Email,
                                      IsActive = u.IsActive
                                  }
                            ).AsNoTracking().FirstOrDefaultAsync();

                if (user == null)
                {
                    return new ApiErrorResult<UserDto>("User không tồn tại.");
                }

                List<AppRoleDto> appRoles = await (from ur in _context.UserRoles
                                                   join r in _context.Roles on ur.RoleId equals r.Id
                                                   where ur.UserId == Guid.Parse(user.Id)
                                                   select new AppRoleDto()
                                                   {
                                                       Code = r.Name,
                                                       Name = r.Name
                                                   }).ToListAsync();

                user.AppRoles = appRoles;

                return new ApiSuccessResult<UserDto>(user);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<UserDto>(ex.Message);
            }
        }
        public async Task<ApiResult<string>> DeleteAvatarByUserId(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiErrorResult<string>("User không tồn tại");
                }

                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    await this.DeleteFile(user.Avatar);
                }

                await _userManager.UpdateAsync(user);

                return new ApiSuccessResult<string>(_configuration[SystemConstants.AppConstants.NoImageAvailable]);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>(ex.Message);
            }
        }
        public async Task<PagedResult<UserDto>> GetStaffFullNameListPaging(ManageUserPagingRequest request)
        {
            try
            {
                var query = from u in _context.Users
                            select new { u };

                if (!string.IsNullOrEmpty(request.TextSearch))
                {
                    query = query.Where(x => x.u.UserName.Contains(request.TextSearch)
                     || (x.u.LastName + " " + x.u.FirstName).Contains(request.TextSearch)
                     || (x.u.Email).Contains(request.TextSearch)
                     );
                }

                //3. Paging
                int totalRow = await query.CountAsync();

                if (request.PageIndex != null)
                {
                    query = query.Skip((request.PageIndex.Value - 1) * request.PageSize)
                                    .Take(request.PageSize);
                }

                var data = await query.Select(x => new UserDto()
                {
                    Id = x.u.Id.ToString(),
                    FullName = x.u.LastName + " " + x.u.FirstName
                }).AsNoTracking().ToListAsync();

                //4. Select and projection
                var pagedResult = new PagedResult<UserDto>()
                {
                    TotalRecords = totalRow,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = data
                };
                return pagedResult;
            }
            catch (Exception ex)
            {
                return new PagedResult<UserDto>()
                {
                    TotalRecords = 0,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = null,
                    Message = ex.Message
                };
            }

        }
        
    }
}

