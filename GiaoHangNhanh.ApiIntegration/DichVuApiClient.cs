using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.DichVus;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IDichVuApiClient
    {
        Task<ApiResult<List<DichVuDto>>> GetAll(ManageDichVuPagingRequest request);
        Task<PagedResult<DichVuDto>> GetManageListPaging(ManageDichVuPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(DichVuRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<DichVuDto>> GetById(int id);
    }
    public class DichVuApiClient : BaseApiClient, IDichVuApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DichVuApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<List<DichVuDto>>> GetAll(ManageDichVuPagingRequest request)
        {
            return await GetListAsync<DichVuDto>($"/api/dichvus/GetAll");
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(DichVuRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/dichvus/addorupdate", request);
        }
        public async Task<PagedResult<DichVuDto>> GetManageListPaging(ManageDichVuPagingRequest request)
        {
            var data = await GetAsync<PagedResult<DichVuDto>>(
                $"/api/dichvus/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<DichVuDto>> GetById(DichVuRequest request)
        {
            var data = await GetAsync<ApiResult<DichVuDto>>($"/api/dichvus/getbyid?Id={request.Id}");

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/dichvus", request);
        }

        public async Task<ApiResult<DichVuDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<DichVuDto>>($"api/dichvus/{id}");
            return data;
        }
    }
}
