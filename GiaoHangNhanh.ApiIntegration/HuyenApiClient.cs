using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Huyens;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IHuyenApiClient
    {
        Task<PagedResult<HuyenDto>> GetManageListPaging(ManageHuyenPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(HuyenRequest request);
        Task<ApiResult<List<HuyenDto>>> GetAll(ManageHuyenPagingRequest request);
        Task<ApiResult<HuyenDto>> GetById(int huyenId);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }

    public class HuyenApiClient : BaseApiClient, IHuyenApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public HuyenApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(HuyenRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/huyens/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/huyens", request);
        }

        public async Task<ApiResult<HuyenDto>> GetById(int huyenId)
        {
            var data = await GetAsync<ApiResult<HuyenDto>>($"/api/huyens/{huyenId}");

            return data;
        }

        public async Task<ApiResult<List<HuyenDto>>> GetAll(ManageHuyenPagingRequest request)
        {
            return await GetListAsync<HuyenDto>($"/api/huyens/GetAll");
        }

        public async Task<PagedResult<HuyenDto>> GetManageListPaging(ManageHuyenPagingRequest request)
        {
            var data = await GetAsync<PagedResult<HuyenDto>>(
                $"/api/huyens/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByTinhId != null ? $"&filterByTinhId={request.FilterByTinhId.Value}" : ""));

            return data;
        }
    }
}
