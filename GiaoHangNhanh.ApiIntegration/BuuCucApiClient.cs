using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.BuuCucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IBuuCucApiClient
    {
        Task<ApiResult<List<BuuCucDto>>> GetAll(ManageBuuCucPagingRequest request);
        Task<PagedResult<BuuCucDto>> GetManageListPaging(ManageBuuCucPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(BuuCucRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<BuuCucDto>> GetById(int id);
        Task<ApiResult<BuuCucDto>> GetByKhuVucId(int khuVucId);
    }
    public class BuuCucApiClient : BaseApiClient, IBuuCucApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BuuCucApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<List<BuuCucDto>>> GetAll(ManageBuuCucPagingRequest request)
        {
            var data = await GetListAsync<BuuCucDto>(
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(BuuCucRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/buucucs/addorupdate", request);
        }
        public async Task<PagedResult<BuuCucDto>> GetManageListPaging(ManageBuuCucPagingRequest request)
        {
            var data = await GetAsync<PagedResult<BuuCucDto>>(
                $"/api/buucucs/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "")+
                (request.FilterByTinhId != null ? $"&FilterByTinhId={request.FilterByTinhId.Value}" : "") +
                (request.FilterByHuyenId != null ? $"&FilterByHuyenId={request.FilterByHuyenId.Value}" : "") +
                (request.FilterByKhuVucId != null ? $"&FilterByKhuVucId={request.FilterByKhuVucId.Value}" : ""));

            return data;
        }

        public async Task<ApiResult<BuuCucDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<BuuCucDto>>($"/api/buucucs/{id}");

            return data;
        }

        public async Task<ApiResult<BuuCucDto>> GetByKhuVucId(int khuVucId)
        {
            var data = await GetAsync<ApiResult<BuuCucDto>>($"/api/buucucs/GetByKhuVucId?khuVucId={khuVucId}");

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/buucucs", request);
        }
    }
}
