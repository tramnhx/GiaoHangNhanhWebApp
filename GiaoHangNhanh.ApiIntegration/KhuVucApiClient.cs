using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IKhuVucApiClient
    {
        Task<PagedResult<KhuVucDto>> GetManageListPaging(ManageKhuVucPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(KhuVucRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<KhuVucDto>> GetById(int id);
        Task<ApiResult<List<KhuVucDto>>> GetAll(ManageKhuVucPagingRequest request);
    }
    public class KhuVucApiClient : BaseApiClient, IKhuVucApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public KhuVucApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(KhuVucRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/KhuVucs/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/KhuVucs", request);
        }

        public async Task<ApiResult<KhuVucDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<KhuVucDto>>($"/api/KhuVucs/{id}");

            return data;
        }

        public async Task<ApiResult<List<KhuVucDto>>> GetAll(ManageKhuVucPagingRequest request)
        {
            return await GetListAsync<KhuVucDto>($"/api/khuvucs/GetAll");
        }

        public async Task<PagedResult<KhuVucDto>> GetManageListPaging(ManageKhuVucPagingRequest request)
        {
            var data = await GetAsync<PagedResult<KhuVucDto>>(
                $"/api/KhuVucs/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByTinhId != null ? $"&filterByTinhId={request.FilterByTinhId.Value}" : "") +
                (request.FilterByHuyenId != null ? $"&filterByHuyenId={request.FilterByHuyenId.Value}" : ""));

            return data;
        }
    }
}
