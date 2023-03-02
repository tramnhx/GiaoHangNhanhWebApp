using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.PhanLoaiHangBatThuongs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiaoHangNhanh.ApiIntegration
{
    public interface IPhanLoaiHangBatThuongApiClient
    {
        Task<ApiResult<List<PhanLoaiHangBatThuongDto>>> GetAll(ManagePhanLoaiHangBatThuongPagingRequest request);
        Task<PagedResult<PhanLoaiHangBatThuongDto>> GetManageListPaging(ManagePhanLoaiHangBatThuongPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(PhanLoaiHangBatThuongRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<PhanLoaiHangBatThuongDto>> GetById(int id);
    }
    public class PhanLoaiHangBatThuongApiClient : BaseApiClient, IPhanLoaiHangBatThuongApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public PhanLoaiHangBatThuongApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<List<PhanLoaiHangBatThuongDto>>> GetAll(ManagePhanLoaiHangBatThuongPagingRequest request)
        {
            return await GetListAsync<PhanLoaiHangBatThuongDto>($"/api/phanloaihangbatthuongs/GetAll");
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(PhanLoaiHangBatThuongRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/phanloaihangbatthuongs/addorupdate", request);
        }

        public async Task<PagedResult<PhanLoaiHangBatThuongDto>> GetManageListPaging(ManagePhanLoaiHangBatThuongPagingRequest request)
        {
            var data = await GetAsync<PagedResult<PhanLoaiHangBatThuongDto>>(
                $"/api/phanloaihangbatthuongs/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<PhanLoaiHangBatThuongDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<PhanLoaiHangBatThuongDto>>($"/api/phanloaihangbatthuongs/{id}");

            return data;
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/phanloaihangbatthuongs", request);
        }
    }
}
