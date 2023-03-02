using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Tinhs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface ITinhApiClient
    {
        Task<PagedResult<TinhDto>> GetManageListPaging(ManageTinhPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(TinhRequest request);
        Task<ApiResult<TinhDto>> GetById(int tinhId);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<List<TinhDto>>> GetAll(ManageTinhPagingRequest request);
    }
    public class TinhApiClient : BaseApiClient, ITinhApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public TinhApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(TinhRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/tinhs/addorupdate", request);
        }

        public async Task<PagedResult<TinhDto>> GetManageListPaging(ManageTinhPagingRequest request)
        {
            var data = await GetAsync<PagedResult<TinhDto>>(
                $"/api/tinhs/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<TinhDto>> GetById(int tinhId)
        {
            var data = await GetAsync<ApiResult<TinhDto>>($"/api/tinhs/{tinhId}");

            return data;
        }

        public async Task<ApiResult<List<TinhDto>>> GetAll(ManageTinhPagingRequest request)
        {
            return await GetListAsync<TinhDto>($"/api/tinhs/GetAll");
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/tinhs", request);
        }
    }
}
