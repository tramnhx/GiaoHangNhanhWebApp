using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuNhapKhos;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface ILichSuNhapKhoApiClient
    {
        Task<PagedResult<LichSuNhapKhoDto>> GetManageListPaging(ManageLichSuNhapKhoPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(LichSuNhapKhoRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class LichSuNhapKhoApiClient : BaseApiClient, ILichSuNhapKhoApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuNhapKhoApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(LichSuNhapKhoRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichsunhapkhos/addorupdate", request);       
        }
        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsunhapkhos", request);
        }
        public async Task<PagedResult<LichSuNhapKhoDto>> GetManageListPaging(ManageLichSuNhapKhoPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuNhapKhoDto>>(
                $"/api/LichSuNhapKhos/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }
}
