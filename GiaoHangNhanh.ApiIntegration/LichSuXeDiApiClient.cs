using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface ILichSuXeDiApiClient
    {
        Task<PagedResult<LichSuXeDiDto>> GetManageListPaging(ManageLichSuXeDiPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(LichSuXeDiRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class LichSuXeDiApiClient : BaseApiClient, ILichSuXeDiApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuXeDiApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(LichSuXeDiRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/LichSuXeDis/addorupdate", request);
        }

        public async Task<PagedResult<LichSuXeDiDto>> GetManageListPaging(ManageLichSuXeDiPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuXeDiDto>>(
                $"/api/LichSuXeDis/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/LichSuXeDis", request);
        }
    }
}
