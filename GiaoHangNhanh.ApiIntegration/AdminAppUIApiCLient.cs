using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.UI.AdminApp;
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
    public interface IAdminAppUIApiClient
    {
        Task<ApiResult<AdminAppLoginDto>> GetLoginObjects();
        Task<ApiResult<AdminAppHeaderDto>> GetHeaderObjects(AdminAppHeaderRequest request);
        Task<ApiResult<AdminAppLeftSideBarDto>> GetLeftSideBarObjects();
    }
    public class AdminAppUIApiCLient : BaseApiClient, IAdminAppUIApiClient
    {
        public AdminAppUIApiCLient(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration):
            base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ApiResult<AdminAppHeaderDto>> GetHeaderObjects(AdminAppHeaderRequest request)
        {
            return await GetAsync<ApiResult<AdminAppHeaderDto>>(
                $"/api/AdminAppUIs/getHeaderObjects?userName={request.UserName}"
            );
        }
        public async Task<ApiResult<AdminAppLoginDto>> GetLoginObjects()
        {
            return await GetAsync<ApiResult<AdminAppLoginDto>>(
                $"/api/AdminAppUIs/getLoginObjects"
            );
        }
        public async Task<ApiResult<AdminAppLeftSideBarDto>> GetLeftSideBarObjects()
        {
            return await GetAsync<ApiResult<AdminAppLeftSideBarDto>>($"/api/AdminAppUIs/getLeftSideBarObjects");
        }
    }
}