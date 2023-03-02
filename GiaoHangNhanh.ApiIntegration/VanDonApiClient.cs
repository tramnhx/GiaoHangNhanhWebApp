using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.VanDons;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
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
    public interface IVanDonApiClient
    {
        Task<ApiResult<int>> AddOrUpdateAsync(VanDonRequest request);
        Task<ApiResult<int>> DeleteByIds(DeleteRequest request);
        Task<ApiResult<VanDonDto>> GetById(int? id);
        Task<ApiResult<VanDonDto>> GetByCode(string code);
        Task<PagedResult<VanDonDto>> GetManageListPaging(ManageVanDonPagingRequest request);
    }

    public class VanDonApiClient : BaseApiClient, IVanDonApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public VanDonApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResult<int>> AddOrUpdateAsync(VanDonRequest request)
        {
            return await BaseAddOrUpdateAsync($"/api/VanDons/addorupdate", request);
        }

        public async Task<ApiResult<int>> DeleteByIds(DeleteRequest request)
        {
            return await BaseDeleteByIds($"/api/VanDons", request);
        }

        public async Task<ApiResult<List<VanDonDto>>> GetAll(ManageVanDonPagingRequest request)
        {
            return await GetListAsync<VanDonDto>($"/api/VanDons/GetAll");
        }

        public async Task<ApiResult<VanDonDto>> GetById(int? id)
        {
            var data = await GetAsync<ApiResult<VanDonDto>>($"/api/VanDons/{id}");

            return data;
        }

        public async Task<ApiResult<VanDonDto>> GetByCode(string code)
        {
            var data = await GetAsync<ApiResult<VanDonDto>>($"/api/vandons/GetByCode?code={code}");

            return data;
        }

        public async Task<PagedResult<VanDonDto>> GetManageListPaging(ManageVanDonPagingRequest request)
        {
            var data = await GetAsync<PagedResult<VanDonDto>>(
                $"/api/VanDons/GetManageListPaging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                (!string.IsNullOrEmpty(request.OrderCol) ? ($"&OrderCol={request.OrderCol}" + $"&OrderDir={request.OrderDir}") : "") +
                (!string.IsNullOrEmpty(request.TextSearch) ? $"&TextSearch={request.TextSearch}" : ""));
            //(request.FilterByBuuCucId != null ? $"&FilterByBuuCucId={request.FilterByBuuCucId.Value}" : "") +
            //(request.FilterByDichVuId != null ? $"&FilterByDichVuId={request.FilterByDichVuId.Value}" : "") +
            //(request.FilterByPhuongThucThanhToanId != null ? $"&FilterByPhuongThucThanhToanId={request.FilterByPhuongThucThanhToanId.Value}" : "") +
            //(request.FilterByCongTyGuiHangId != null ? $"&FilterByCongTyGuiHangId={request.FilterByCongTyGuiHangId.Value}" : ""));

            return data;
        }
    }
}
