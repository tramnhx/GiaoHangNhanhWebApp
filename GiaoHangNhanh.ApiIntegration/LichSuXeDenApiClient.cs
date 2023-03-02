using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuXeDens;
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
    public interface ILichSuXeDenApiClient
    {
        Task<PagedResult<LichSuXeDenDto>> GetManageListPaging(ManageLichSuXeDenPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(LichSuXeDenRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class LichSuXeDenApiClient : BaseApiClient, ILichSuXeDenApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuXeDenApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(LichSuXeDenRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichsuxedens/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsuxedens", request);
        }

        public async Task<PagedResult<LichSuXeDenDto>> GetManageListPaging(ManageLichSuXeDenPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuXeDenDto>>(
                $"/api/lichsuxedens/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }

}
