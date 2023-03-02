using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IAppRoleApiClient
    {
        Task<ApiResult<List<AppRoleDto>>> GetAll(ManageAppRolePagingRequest request);
        Task<PagedResult<AppRoleDto>> GetManageListPaging(ManageAppRolePagingRequest request);
        Task<ApiResult<string>> AddOrUpdateAsync(AppRoleRequest request);

        Task<ApiResult<AppRoleDto>> GetById(int appRoleId, int languageId);

        Task<ApiResult<int>> DeleteByIds(AppRoleDeleteRequest request);
    }
    public class AppRoleApiClient : BaseApiClient, IAppRoleApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AppRoleApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<string>> AddOrUpdateAsync(AppRoleRequest request)
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

                var response = await client.PostAsync($"/api/appRoles/addorupdate", content);
                return JsonConvert.DeserializeObject<ApiResult<string>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>() { IsSuccessed = false, Message = ex.Message };
            }
        }
        public async Task<ApiResult<List<AppRoleDto>>> GetAll(ManageAppRolePagingRequest request)
        {
            var data = await GetListAsync<AppRoleDto>(
                $"/api/approles/getall?pageIndex={request.PageIndex}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
        public async Task<PagedResult<AppRoleDto>> GetManageListPaging(ManageAppRolePagingRequest request)
        {
            var data = await GetAsync<PagedResult<AppRoleDto>>(
                $"/api/appRoles/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<AppRoleDto>> GetById(int appRoleId, int languageId)
        {
            var data = await GetAsync<ApiResult<AppRoleDto>>($"/api/appRoles/{appRoleId}/{languageId}");

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(AppRoleDeleteRequest request)
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

                var response = await client.PostAsync($"/api/appRoles/deletebyids", content);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiSuccessResult<int>();
                }
                else
                {
                    return new ApiErrorResult<int>() { IsSuccessed = false, Message = "Lỗi" };
                }
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>() { IsSuccessed = false, Message = ex.Message };
            }
        }
    }
}