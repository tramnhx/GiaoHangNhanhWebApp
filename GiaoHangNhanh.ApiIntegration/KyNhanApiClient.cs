using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IKyNhanApiClient
    {
        Task<ApiResult<List<KyNhanDto>>> GetAll(ManageKyNhanPagingRequest request);
        Task<PagedResult<KyNhanDto>> GetManageListPaging(ManageKyNhanPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(KyNhanRequest request);

        Task<ApiResult<KyNhanDto>> GetById(int Id);

        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class KyNhanApiClient : BaseApiClient, IKyNhanApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public KyNhanApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<List<KyNhanDto>>> GetAll(ManageKyNhanPagingRequest request)
        {
            return await GetListAsync<KyNhanDto>($"/api/kynhans/GetAll");
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(KyNhanRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/kynhans/addorupdate", request);
        }
        public async Task<PagedResult<KyNhanDto>> GetManageListPaging(ManageKyNhanPagingRequest request)
        {
            var data = await GetAsync<PagedResult<KyNhanDto>>(
                $"/api/kynhans/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByDMBuuCuc != null ? $"&FilterByDMBuuCuc={request.FilterByDMBuuCuc.Value}" : ""));

            return data;
        }

        public async Task<ApiResult<KyNhanDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<KyNhanDto>>($"/api/kynhans/{id}");

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/kynhans", request);
        }
    }
}
