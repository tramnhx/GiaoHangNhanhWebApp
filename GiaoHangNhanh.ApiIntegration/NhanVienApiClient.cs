using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.NhanViens;
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
    public interface INhanVienApiClient
    {
        Task<ApiResult<List<NhanVienDto>>> GetAll(ManageNhanVienPagingRequest request);
        Task<PagedResult<NhanVienDto>> GetManageListPaging(ManageNhanVienPagingRequest request);
        Task<ApiResult<int>> AddOrUpdateAsync(NhanVienRequest request);
        Task<ApiResult<NhanVienDto>> GetById(int id);

        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
    }
    public class NhanVienApiClient : BaseApiClient, INhanVienApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public NhanVienApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ApiResult<int>> AddOrUpdateAsync(NhanVienRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/nhanviens/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/nhanviens", request);
        }

        public async Task<ApiResult<List<NhanVienDto>>> GetAll(ManageNhanVienPagingRequest request)
        {
            var data = await GetListAsync<NhanVienDto>(
                $"/api/nhanviens/getall?pageIndex={request.PageIndex}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));

            return data;
        }

        public async Task<ApiResult<NhanVienDto>> GetById(int id)
        {
            var data = await GetAsync<ApiResult<NhanVienDto>>($"/api/nhanviens/{id}");

            return data;
        }

        public async Task<PagedResult<NhanVienDto>> GetManageListPaging(ManageNhanVienPagingRequest request)
        {
            var data = await GetAsync<PagedResult<NhanVienDto>>(
                $"/api/nhanviens/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : "") +
                (request.FilterByBuuCucLamViecId != null ? $"&FilterByBuuCucLamViecId={request.FilterByBuuCucLamViecId.Value}" : "") +
                (request.FilterByGenderId != null ? $"&FilterByGenderId={request.FilterByGenderId.Value}" : ""));

            return data;
        }
    }
}
