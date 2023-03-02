using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuGuiHangs;
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
    public interface ILichSuGuiHangApiClient
    {
        //Task<ApiResult<List<LichSuGuiHangDto>>> GetAll(ManageLichSuGuiHangPagingRequest request);
        Task<PagedResult<LichSuGuiHangDto>> GetManageListPaging(ManageLichSuGuiHangPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(LichSuGuiHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        //Task<ApiResult<LichSuGuiHangDto>> GetById(int id);
    }
    public class LichSuGuiHangApiClient : BaseApiClient, ILichSuGuiHangApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuGuiHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(LichSuGuiHangRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichsuguihangs/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsuguihangs", request);
        }

        public async Task<ApiResult<List<LichSuGuiHangDto>>> GetAll(ManageLichSuGuiHangPagingRequest request)
        {
            return await GetListAsync<LichSuGuiHangDto>($"/api/lichsuguihangs/GetAll");
        }

        public async Task<ApiResult<LichSuGuiHangDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<LichSuGuiHangDto>>($"/api/lichsuguihangs/{id}");

            return data;
        }

        public async Task<PagedResult<LichSuGuiHangDto>> GetManageListPaging(ManageLichSuGuiHangPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuGuiHangDto>>(
              $"/api/lichsuguihangs/GetManageListPaging?pageIndex={request.PageIndex}" +
              $"&pageSize={request.PageSize}" +
              (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
              (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }

}
