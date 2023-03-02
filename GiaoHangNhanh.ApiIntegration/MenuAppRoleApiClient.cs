using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.MenuAppRoles;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IMenuAppRoleApiClient
    {
        Task<ApiResult<List<MenuAppRoleDto>>> GetAll(ManageMenuAppRolePagingRequest request);
        Task<ApiResult<int>> Save(MenuAppRoleRequest request);
    }
    public class MenuAppRoleApiClient : BaseApiClient, IMenuAppRoleApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MenuAppRoleApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<List<MenuAppRoleDto>>> GetAll(ManageMenuAppRolePagingRequest request)
        {
            var data = await GetListAsync<MenuAppRoleDto>(
                $"/api/menuapproles/getall?" +
                (!string.IsNullOrEmpty(request.AppRoleId) ? $"AppRoleId={request.AppRoleId}" : "") +
                (request.MenuId != null ? $"MenuId={request.MenuId.Value}" : "") +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<int>> Save(MenuAppRoleRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/MenuAppRoles/save", request);
        }
    }
}
