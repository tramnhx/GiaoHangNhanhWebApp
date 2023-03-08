using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Genders;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IGenderApiClient
    {
        Task<ApiResult<List<GenderDto>>> GetAll(ManageGenderPagingRequest request);
        Task<PagedResult<GenderDto>> GetManageListPaging(ManageGenderPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(GenderRequest request);

        Task<ApiResult<GenderDto>> GetById(int genderId, int languageId);

        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class GenderApiClient : BaseApiClient, IGenderApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public GenderApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<List<GenderDto>>> GetAll(ManageGenderPagingRequest request)
        {
            return await GetListAsync<GenderDto>($"/api/genders/getAll");
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(GenderRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/genders/addorupdate", request);
        }
        public async Task<PagedResult<GenderDto>> GetManageListPaging(ManageGenderPagingRequest request)
        {
            var data = await GetAsync<PagedResult<GenderDto>>(
                $"/api/genders/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<GenderDto>> GetById(int genderId, int languageId)
        {
            var data = await GetAsync<ApiResult<GenderDto>>($"/api/genders/{genderId}/{languageId}");

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/genders", request);
        }
    }
}
