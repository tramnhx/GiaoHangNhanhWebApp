using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IAdminAppUIApiClient _adminAppUIApiClient;
        public LoginController(IUserApiClient userApiClient, IConfiguration configuration,
                                IAdminAppUIApiClient adminAppUIApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _adminAppUIApiClient = adminAppUIApiClient;

        }
        [HttpGet]
        public async Task<IActionResult> Index(string message)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var model = new LoginViewModel()
            {
                Logo = string.Empty,
                Message = message
            };

            var adminAppUIApiClient = await _adminAppUIApiClient.GetLoginObjects();
            if (adminAppUIApiClient.IsSuccessed)
            {
                model.Logo = _configuration[SystemConstants.AppConstants.BaseAddress] + adminAppUIApiClient.ResultObj.Logo;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Message = "Tên truy cập hoặc mật khẩu không hợp lệ.";
                return View(model);
            }

            var result = await _userApiClient.Authencate(new LoginRequest()
            {
                UserName = model.UserName,
                Password = model.Password,
                RememberMe = model.RememberMe
            });

            if (!result.IsSuccessed)
            {
                model.Message = result.Message;
                return View(model);
            }
            else
            {
                if (result.ResultObj == null)
                {
                    model.Message = result.Message;
                    return View(model);
                }
            }

            var userPrincipal = this.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false
            };

            string userId = userPrincipal.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault();

            HttpContext.Session.SetString(SystemConstants.AppConstants.Token, result.ResultObj);
            HttpContext.Session.SetString(SystemConstants.UserConstants.UserId, userId);

            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            return RedirectToAction("Index", "Home");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
    }
}
