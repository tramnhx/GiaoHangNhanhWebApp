using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuThaoBaos;
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
    public interface ILichSuThaoBaoApiClient
    {
        //Task<ApiResult<List<LichSuThaoBaoDto>>> GetAll(ManageLichSuThaoBaoPagingRequest request);
        Task<PagedResult<LichSuThaoBaoDto>> GetManageListPaging(ManageLichSuThaoBaoPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(LichSuThaoBaoRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        //Task<ApiResult<LichSuThaoBaoDto>> GetById(int id);
    }
    public class LichSuThaoBaoApiClient : BaseApiClient, ILichSuThaoBaoApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuThaoBaoApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(LichSuThaoBaoRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichsuthaobaos/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichsuthaobaos", request);
        }

        public async Task<PagedResult<LichSuThaoBaoDto>> GetManageListPaging(ManageLichSuThaoBaoPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuThaoBaoDto>>(
               $"/api/lichsuthaobaos/GetManageListPaging?pageIndex={request.PageIndex}" +
               $"&pageSize={request.PageSize}" +
               (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
               (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }
    }
}
