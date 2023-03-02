using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyChuyenHoans;
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
    public interface IDangKyChuyenHoanApiClient
    {
        Task<ApiResult<int>> AddOrUpdateAsync(DangKyChuyenHoanRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<PagedResult<DangKyChuyenHoanDto>> GetManageListPaging(ManageDangKyChuyenHoanPagingRequest request);
    }
    public class DangKyChuyenHoanApiClient : BaseApiClient, IDangKyChuyenHoanApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DangKyChuyenHoanApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(DangKyChuyenHoanRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/dangkychuyenhoans/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/dangkychuyenhoans", request);
        }

        public async Task<PagedResult<DangKyChuyenHoanDto>> GetManageListPaging(ManageDangKyChuyenHoanPagingRequest request)
        {
            var data = await GetAsync<PagedResult<DangKyChuyenHoanDto>>(
                $"/api/dangkychuyenhoans/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") );
      

            return data;
        }
    }
}

