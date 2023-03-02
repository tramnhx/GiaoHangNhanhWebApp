
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);
        Task<ApiResult<string>> ChangePassword(ChangePasswordRequest request);
        Task<PagedResult<UserDto>> GetStaffManageListPaging(ManageUserPagingRequest request);
        Task<ApiResult<UserDto>> GetStaffSecurityByUserId(UserRequest request);
        Task<ApiResult<string>> AddOrUpdateStaffSecurity(UserRequest request);
        Task<ApiResult<string>> AddOrUpdateStaffProfileDetail(UserRequest request);
        Task<ApiResult<UserDto>> GetStaffProfileDetailByUserId(UserRequest request);
        Task<ApiResult<int>> DeleteByIds(UserDeleteRequest request);
    }
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<string>> AddOrUpdateStaffProfileDetail(UserRequest request)
        {
            try
            {
                var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppConstants.Token);

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppConstants.BaseAddress]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);

                var requestContent = new MultipartFormDataContent();

                if (request.Id != null)
                {
                    requestContent.Add(new StringContent(request.Id), "id");
                }


                if (request.Avatar != null)
                {
                    byte[] data;
                    using (var br = new BinaryReader(request.Avatar.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)request.Avatar.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "Avatar", request.Avatar.FileName);
                }
                if (!string.IsNullOrEmpty(request.MaNhanVien))
                {
                    requestContent.Add(new StringContent(request.MaNhanVien), "MaNhanVien");
                }
                if (!string.IsNullOrEmpty(request.FirstName))
                {
                    requestContent.Add(new StringContent(request.FirstName), "FirstName");
                }

                if (!string.IsNullOrEmpty(request.LastName))
                {
                    requestContent.Add(new StringContent(request.LastName), "LastName");
                }

                if (!string.IsNullOrEmpty(request.Dob))
                {
                    requestContent.Add(new StringContent(request.Dob), "Dob");
                }

                if (!string.IsNullOrEmpty(request.PhoneNumber))
                {
                    requestContent.Add(new StringContent(request.PhoneNumber), "PhoneNumber");
                }

                if (!string.IsNullOrEmpty(request.Address))
                {
                    requestContent.Add(new StringContent(request.Address), "Address");
                }

                if (!string.IsNullOrEmpty(request.CreatedUserId))
                {
                    requestContent.Add(new StringContent(request.CreatedUserId), "CreatedUserId");
                }

                if (!string.IsNullOrEmpty(request.ModifiedUserId))
                {
                    requestContent.Add(new StringContent(request.ModifiedUserId), "ModifiedUserId");
                }

                if (!string.IsNullOrEmpty(request.LeaveDate))
                {
                    requestContent.Add(new StringContent(request.LeaveDate), "LeaveDate");
                }

                if (!string.IsNullOrEmpty(request.StartingDate))
                {
                    requestContent.Add(new StringContent(request.StartingDate), "StartingDate");
                }

                var response = await client.PostAsync($"api/users/addorupdatestaffprofiledetail", requestContent);
                return JsonConvert.DeserializeObject<ApiResult<string>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>() { IsSuccessed = false, Message = ex.Message };
            }
        }

        public async Task<ApiResult<string>> AddOrUpdateStaffSecurity(UserRequest request)
        {
            try
            {
                var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppConstants.Token);

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppConstants.BaseAddress]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


                string strPayload = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"api/users/addorupdatestaffsecurity", content);
                return JsonConvert.DeserializeObject<ApiResult<string>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>() { IsSuccessed = false, Message = ex.Message };
            }
        }
        public async Task<ApiResult<string>> Authencate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<string>>(await response.Content.ReadAsStringAsync());
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<ApiResult<string>> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppConstants.Token);

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppConstants.BaseAddress]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


                string strPayload = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"api/users/ChangePassword", content);
                return JsonConvert.DeserializeObject<ApiResult<string>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>(ex.Message);
            }
        }

        public async Task<ApiResult<int>> DeleteByIds(UserDeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/users", request);
        }

        public async Task<PagedResult<UserDto>> GetStaffManageListPaging(ManageUserPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var sessions = _httpContextAccessor.HttpContext.Session.GetString("Token");

            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await client.GetAsync($"/api/users/GetStaffManageListPaging?pageIndex=" +
                $"{request.PageIndex}&pageSize={request.PageSize}&textSearch={request.TextSearch}");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<PagedResult<UserDto>>(body);
            return users;
        }

        public async Task<ApiResult<UserDto>> GetStaffProfileDetailByUserId(UserRequest request)
        {
            return await GetAsync<ApiResult<UserDto>>($"/api/users/getstaffprofiledetailbyuserid?id={request.Id}");
        }

        public async Task<ApiResult<UserDto>> GetStaffSecurityByUserId(UserRequest request)
        {
            return await GetAsync<ApiResult<UserDto>>($"/api/users/getstaffsecuritybyuserid?Id={request.Id}");
        }
    }
}
