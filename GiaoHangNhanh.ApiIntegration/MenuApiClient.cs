using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Menus;
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
    public interface IMenuApiClient
    {
        Task<ApiResult<List<MenuDto>>> GetAll(ManageMenuPagingRequest request);
        Task<PagedResult<MenuDto>> GetManageListPaging(ManageMenuPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(MenuRequest request);
        Task<ApiResult<MenuDto>> GetById(MenuRequest request);

        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class MenuApiClient : BaseApiClient, IMenuApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MenuApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(MenuRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/menus/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/menus", request);
        }

        public async Task<ApiResult<List<MenuDto>>> GetAll(ManageMenuPagingRequest request)
        {
            var data = await GetListAsync<MenuDto>(
                $"/api/menus/getall?pageIndex={request.PageIndex}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<MenuDto>> GetById(MenuRequest request)
        {
            var data = await GetAsync<ApiResult<MenuDto>>($"/api/menus/getbyid?Id={request.Id}");

            return data;
        }

        public async Task<PagedResult<MenuDto>> GetManageListPaging(ManageMenuPagingRequest request)
        {
            var data = await GetAsync<PagedResult<MenuDto>>(
                $"/api/menus/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }
}
