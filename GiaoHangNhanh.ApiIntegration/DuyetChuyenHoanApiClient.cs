using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DuyetChuyenHoans;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IDuyetChuyenHoanApiClient
    {
        Task<ApiResult<int>> AddOrUpdateAsync(DuyetChuyenHoanRequest request);
        Task<PagedResult<DuyetChuyenHoanDto>> GetManageListPaging(ManageDuyetChuyenHoanPagingRequest request);
    }
    public class DuyetChuyenHoanApiClient : BaseApiClient, IDuyetChuyenHoanApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DuyetChuyenHoanApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(DuyetChuyenHoanRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/duyetchuyenhoans/addorupdate", request);
        }

        public async Task<PagedResult<DuyetChuyenHoanDto>> GetManageListPaging(ManageDuyetChuyenHoanPagingRequest request)
        {
            var data = await GetAsync<PagedResult<DuyetChuyenHoanDto>>(
                $"/api/duyetchuyenhoans/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByDangKyChuyenHoanId != null ? $"&FilterByDangKyChuyenHoan={request.FilterByDangKyChuyenHoanId.Value}" : "") +
                (request.FilterByVanDonId != null ? $"&FilterByVanDonId={request.FilterByVanDonId.Value}" : ""));
            return data;
        }
    }
}

