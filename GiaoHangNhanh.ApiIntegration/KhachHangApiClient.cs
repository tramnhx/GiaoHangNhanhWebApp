using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhachHangs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IKhachHangApiClient
    {
        Task<ApiResult<int>> AddOrUpdateAsync(KhachHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<List<KhachHangDto>>> GetAll(ManageKhachHangPagingRequest request);
        Task<ApiResult<KhachHangDto>> GetById(int id);
        Task<PagedResult<KhachHangDto>> GetManageListPaging(ManageKhachHangPagingRequest request);

    }
    public class KhachHangApiClient : BaseApiClient, IKhachHangApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public KhachHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(KhachHangRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/khachhangs/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/khachhangs", request);
        }

        public async Task<ApiResult<List<KhachHangDto>>> GetAll(ManageKhachHangPagingRequest request)
        {
            return await GetListAsync<KhachHangDto>($"/api/khachhangs/GetAll");
        }

        public async Task<ApiResult<KhachHangDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<KhachHangDto>>($"/api/khachhangs/{id}");

            return data;
        }

        public async Task<PagedResult<KhachHangDto>> GetManageListPaging(ManageKhachHangPagingRequest request)
        {
            var data = await GetAsync<PagedResult<KhachHangDto>>(
                $"/api/khachhangs/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }
}
