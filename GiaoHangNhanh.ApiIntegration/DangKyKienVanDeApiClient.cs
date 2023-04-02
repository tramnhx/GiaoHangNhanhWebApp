using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.DangKyKienVanDes;
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
    public interface IDangKyKienVanDeApiClient
    {
        Task<PagedResult<DangKyKienVanDeDto>> GetManageListPaging(ManageDangKyKienVanDePagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(DangKyKienVanDeRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<DangKyKienVanDeDto>> GetById(int id);
    }
    public class DangKyKienVanDeApiClient : BaseApiClient, IDangKyKienVanDeApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public DangKyKienVanDeApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(DangKyKienVanDeRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/dangkykienvandes/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/dangkykienvandes", request);
        }

        public async Task<ApiResult<List<DangKyKienVanDeDto>>> GetAll(ManageDangKyKienVanDePagingRequest request)
        {
            return await GetListAsync<DangKyKienVanDeDto>($"/api/dangkykienvandes/GetAll");
        }

        public async Task<ApiResult<DangKyKienVanDeDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<DangKyKienVanDeDto>>($"/api/dangkykienvandes/{id}");

            return data;
        }

        public async Task<PagedResult<DangKyKienVanDeDto>> GetManageListPaging(ManageDangKyKienVanDePagingRequest request)
        {
            var data = await GetAsync<PagedResult<DangKyKienVanDeDto>>(
              $"/api/dangkykienvandes/GetManageListPaging?pageIndex={request.PageIndex}" +
              $"&pageSize={request.PageSize}" +
              (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
              (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }

}
