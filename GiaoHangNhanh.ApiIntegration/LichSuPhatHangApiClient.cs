using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.LichSuPhatHangs;
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
    public interface ILichSuPhatHangApiClient
    {
        Task<PagedResult<LichSuPhatHangDto>> GetManageListPaging(ManageLichSuPhatHangPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(LichSuPhatHangRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<LichSuPhatHangDto>> GetById(int id);
    }
    public class LichSuPhatHangApiClient : BaseApiClient, ILichSuPhatHangApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public LichSuPhatHangApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(LichSuPhatHangRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/lichSuPhatHangs/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/lichSuPhatHangs", request);
        }

        public async Task<ApiResult<List<LichSuPhatHangDto>>> GetAll(ManageLichSuPhatHangPagingRequest request)
        {
            return await GetListAsync<LichSuPhatHangDto>($"/api/lichSuPhatHangs/GetAll");
        }

        public async Task<ApiResult<LichSuPhatHangDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<LichSuPhatHangDto>>($"/api/LichSuPhatHangs/{id}");

            return data;
        }

        public async Task<PagedResult<LichSuPhatHangDto>> GetManageListPaging(ManageLichSuPhatHangPagingRequest request)
        {
            var data = await GetAsync<PagedResult<LichSuPhatHangDto>>(
              $"/api/lichSuPhatHangs/GetManageListPaging?pageIndex={request.PageIndex}" +
              $"&pageSize={request.PageSize}" +
              (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
              (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByNhanVienId != null ? $"&FilterByNhanVienId={request.FilterByNhanVienId.Value}" : ""));

            return data;
        }
    }

}
