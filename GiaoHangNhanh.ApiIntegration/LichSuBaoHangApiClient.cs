using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuBaoHangs;
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
    public interface ILichSuBaoHangApiClient
    {
        Task<PagedResult<LichSuBaoHangDto>> GetManageBaoHangListPaging(ManageLichSuBaoHangPagingRequest request);
        Task<PagedResult<LichSuBaoHangDto>> GetManageVanDonInBaoListPaging(ManageLichSuBaoHangPagingRequest request);
        Task<ApiResult<int>> DongBaoHangAsync(LichSuBaoHangRequest request);
        Task<ApiResult<int>> ThaoBaoHangAsync(LichSuBaoHangRequest request);
        Task<ApiResult<int>> AddVanDonInBaoAsync(VanDonInBaoRequest request);
        Task<ApiResult<LichSuBaoHangDto>> GetById(int baoHangId);
        Task<ApiResult<int>> LayHangRaBao(DeleteRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<int>> DeleteBaoHangByIds(DeleteRequest request);
    }
    public class LichSuBaoHangApiClient : BaseApiClient, ILichSuBaoHangApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        public LichSuBaoHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<int>> DongBaoHangAsync(LichSuBaoHangRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichsubaohangs/dongbaohang", request);
        }
        public async Task<ApiResult<int>> ThaoBaoHangAsync(LichSuBaoHangRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichsubaohangs/thaobaohang", request);
        }

        public async Task<ApiResult<int>> LayHangRaBao(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsubaohangs/layhangrabao", request);
        }
        public async Task<ApiResult<int>> DeleteBaoHangByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsubaohangs/deletebaohangbyids", request);
        }
        public async Task<ApiResult<int>> AddVanDonInBaoAsync(VanDonInBaoRequest request)
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

                var response = await client.PostAsync($"/api/lichsubaohangs/addvandoninbao", content);
                return JsonConvert.DeserializeObject<ApiResult<int>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.Message);
            }
        }

        public async Task<ApiResult<LichSuBaoHangDto>> GetById(int baoHangId)
        {
            var data = await GetAsync<ApiResult<LichSuBaoHangDto>>($"/api/lichsubaohangs/{baoHangId}");

            return data;
        }
        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsubaohangs", request);
        }

        public async Task<PagedResult<LichSuBaoHangDto>> GetManageBaoHangListPaging(ManageLichSuBaoHangPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuBaoHangDto>>(
                $"/api/lichsubaohangs/GetManageBaoHangListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
        public async Task<PagedResult<LichSuBaoHangDto>> GetManageVanDonInBaoListPaging(ManageLichSuBaoHangPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuBaoHangDto>>(
                $"/api/lichsubaohangs/GetManageVanDonInBaoListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByMaSealBao != null ? $"&filterByMaSealBao={request.FilterByMaSealBao}" : ""));

            return data;
        }

    }

}
