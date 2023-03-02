using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.CongTyGuiHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface ICongTyGuiHangApiClient
    {
        Task<ApiResult<int>> AddOrUpdateAsync(CongTyGuiHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<List<CongTyGuiHangDto>>> GetAll(ManageCongTyGuiHangPagingRequest request);
        Task<ApiResult<CongTyGuiHangDto>> GetById(int id);
        Task<PagedResult<CongTyGuiHangDto>> GetManageListPaging(ManageCongTyGuiHangPagingRequest request);
    }

    public class CongTyGuiHangApiClient : BaseApiClient, ICongTyGuiHangApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CongTyGuiHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(CongTyGuiHangRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/CongTyGuiHangs/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/CongTyGuiHangs", request);
        }

        public async Task<ApiResult<List<CongTyGuiHangDto>>> GetAll(ManageCongTyGuiHangPagingRequest request)
        {
            return await GetListAsync<CongTyGuiHangDto>($"/api/CongTyGuiHangs/GetAll");
        }

        public async Task<ApiResult<CongTyGuiHangDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<CongTyGuiHangDto>>($"/api/CongTyGuiHangs/{id}");

            return data;
        }

        public async Task<PagedResult<CongTyGuiHangDto>> GetManageListPaging(ManageCongTyGuiHangPagingRequest request)
        {
            var data = await GetAsync<PagedResult<CongTyGuiHangDto>>(
                $"/api/CongTyGuiHangs/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByTinhId != null ? $"&FilterByTinhId={request.FilterByTinhId.Value}" : "") +
                (request.FilterByHuyenId != null ? $"&FilterByHuyenId={request.FilterByHuyenId.Value}" : "") +
                (request.FilterByKhuVucId != null ? $"&FilterByKhuVucId={request.FilterByKhuVucId.Value}" : ""));

            return data;
        }
    }
}
