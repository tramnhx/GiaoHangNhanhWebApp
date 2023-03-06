using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuHangDens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface ILichSuHangDenApiClient
    {
        Task<PagedResult<LichSuHangDenDto>> GetManageListPaging(ManageLichSuHangDenPagingRequest request);
        Task<ApiResult<int>> AddAsync(LichSuHangDenRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class LichSuHangDenApiClient : BaseApiClient, ILichSuHangDenApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuHangDenApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddAsync(LichSuHangDenRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/LichSuHangDens/Add", request);
        }

        public async Task<PagedResult<LichSuHangDenDto>> GetManageListPaging(ManageLichSuHangDenPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuHangDenDto>>(
                $"/api/LichSuHangDens/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByBuuCucId != null ? $"&FilterByBuuCucId={request.FilterByBuuCucId.Value}" : ""));

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/LichSuHangDens", request);
        }
    }
}
