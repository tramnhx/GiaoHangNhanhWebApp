using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhuongThucThanhToans;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.Utilities.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IPhuongThucThanhToanApiClient
    {
        Task<ApiResult<List<PhuongThucThanhToanDto>>> GetAll(ManagePhuongThucThanhToanPagingRequest request);
        Task<PagedResult<PhuongThucThanhToanDto>> GetManageListPaging(ManagePhuongThucThanhToanPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(PhuongThucThanhToanRequest request);
        Task<ApiResult<PhuongThucThanhToanDto>> GetById(int Id, int languageId);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class PhuongThucThanhToanApiClient : BaseApiClient, IPhuongThucThanhToanApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PhuongThucThanhToanApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(PhuongThucThanhToanRequest request)
        {
            try
            {
                var sessions = _httpContextAccessor
                .HttpContext
                .Session
                .GetString(SystemConstants.AppConstants.Token);

                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri(_configuration[SystemConstants.AppConstants.BaseAddress]);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);


                string strPayload = JsonConvert.SerializeObject(request);
                HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"/api/PhuongThucThanhToans/addorupdate", content);
                return JsonConvert.DeserializeObject<ApiResult<int>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>() { IsSuccessed = false, Message = ex.Message };
            }
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/PhuongThucThanhToans", request);
        }

        public async Task<ApiResult<List<PhuongThucThanhToanDto>>> GetAll(ManagePhuongThucThanhToanPagingRequest request)
        {
            return await GetListAsync<PhuongThucThanhToanDto>($"/api/phuongthucthanhtoans/GetAll");
        }

        public async Task<ApiResult<PhuongThucThanhToanDto>> GetById(int Id, int languageId)
        {
            var data = await GetAsync<ApiResult<PhuongThucThanhToanDto>>($"/api/PhuongThucThanhToans/{Id}/{languageId}");

            return data;
        }

        public async Task<PagedResult<PhuongThucThanhToanDto>> GetManageListPaging(ManagePhuongThucThanhToanPagingRequest request)
        {
            var data = await GetAsync<PagedResult<PhuongThucThanhToanDto>>(
                $"/api/PhuongThucThanhToans/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }


    }
}
