using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuChuyenHangs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface ILichSuChuyenHangApiClient
    {
        Task<PagedResult<LichSuChuyenHangDto>> GetManageListPaging(ManageLichSuChuyenHangPagingRequest request);
        Task<ApiResult<int>> AddXeDiOrXeDenAsync(LichSuChuyenHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class LichSuChuyenHangApiClient : BaseApiClient, ILichSuChuyenHangApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuChuyenHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddXeDiOrXeDenAsync(LichSuChuyenHangRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/LichSuChuyenHangs/AddXeDiOrXeDen", request);
        }

        public async Task<PagedResult<LichSuChuyenHangDto>> GetManageListPaging(ManageLichSuChuyenHangPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuChuyenHangDto>>(
                $"/api/LichSuChuyenHangs/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByBuuCucId != null ? $"&FilterByBuuCucId={request.FilterByBuuCucId.Value}" : ""));

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/LichSuChuyenHangs", request);
        }
    }
}
