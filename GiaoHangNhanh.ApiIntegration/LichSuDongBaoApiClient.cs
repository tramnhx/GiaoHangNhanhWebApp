using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuDongBaos;
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
    public interface ILichSuDongBaoApiClient
    {
        //Task<ApiResult<List<LichSuDongBaoDto>>> GetAll(ManageLichSuDongBaoPagingRequest request);
        Task<PagedResult<LichSuDongBaoDto>> GetManageListPaging(ManageLichSuDongBaoPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(LichSuDongBaoRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        //Task<ApiResult<LichSuDongBaoDto>> GetById(int id);
    }
    public class LichSuDongBaoApiClient : BaseApiClient, ILichSuDongBaoApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuDongBaoApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(LichSuDongBaoRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichsudongbaos/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsudongbaos", request);
        }

        public async Task<PagedResult<LichSuDongBaoDto>> GetManageListPaging(ManageLichSuDongBaoPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuDongBaoDto>>(
               $"/api/lichsudongbaos/GetManageListPaging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" +
               (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
               (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }
}
