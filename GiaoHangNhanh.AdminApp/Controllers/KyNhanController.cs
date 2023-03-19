using BHSNetCoreLib.ExcelUtil;
using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
using GiaoHangNhanh.DAL.Entities.EntityDto.Manipulation.KyNhans;
using GiaoHangNhanh.DAL.Entities.EntityDto.System.Users;
using GiaoHangNhanh.Utilities.Constants;
using GiaoHangNhanh.Utilities.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiaoHangNhanh.AdminApp.Controllers
{
    public class KyNhanController : BaseController
    {
        private readonly IKyNhanApiClient _kyNhanApiClient;
        public KyNhanController(IKyNhanApiClient kyNhanApiClient)
        {
            _kyNhanApiClient = kyNhanApiClient;
        }

        public IActionResult Index()
        {
            KyNhanViewModel model = new KyNhanViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Ký Nhận";
            model.Breadcrumbs = new List<string>() { "Thao tác", "Danh mục hệ thống", "Ký Nhận" };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch, string filterByDMBuuCuc, string filterByVanDon)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            int filterByDMBuuCucValue,filterByVanDonValue;

            var request = new ManageKyNhanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",

                FilterByDMBuuCuc = int.TryParse(filterByDMBuuCuc, out filterByDMBuuCucValue) ? filterByDMBuuCucValue : new Nullable<int>(),
                FilterByVanDon = int.TryParse(filterByVanDon, out filterByVanDonValue) ? filterByDMBuuCucValue : new Nullable<int>(),
            };

            var kyNhanApiClient = await _kyNhanApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = kyNhanApiClient.TotalRecords,
                recordsTotal = kyNhanApiClient.TotalRecords,
                data = kyNhanApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _kyNhanApiClient.DeleteByIds(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<KyNhanRequest> rq)
        {
            ApiResult<int> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));

            if (rq != null)
            {
                if (rq.Id == null)
                {
                    rq.Data.CreatedUserId = userGuid;
                    rq.Data.ModifiedUserId = userGuid;
                    rq.Data.NhanVienPhat = userGuid;
                }
                else
                {
                    rq.Data.ModifiedUserId = userGuid;
                    rq.Data.Id = rq.Id.Value;
                }
                result = await _kyNhanApiClient.AddOrUpdateAsync(rq.Data);
            }
            else
            {
                result = new ApiResult<int>()
                {
                    IsSuccessed = false,
                    Message = "Không có dữ liệu"
                };
            }

            return Ok(result);
        }


        

        public async Task<IActionResult> Filter(string textSearch, int? filterByDMBuuCuc, int? filterByVanDon)
        {

            var request = new ManageKyNhanPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc",
                FilterByDMBuuCuc = filterByDMBuuCuc,
                FilterByVanDon = filterByVanDon
            };

            var kyNhanApiClient = await _kyNhanApiClient.GetManageListPaging(request);
            return Ok(kyNhanApiClient);
        }
        public async Task<FileResult> ExportToExcel()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataColumn dc;
            List<string> columnsToTake;
            System.Reflection.PropertyInfo propertyInfo;
            List<BaseDto> exportedFields = new List<BaseDto>();

            exportedFields.Add(new BaseDto()
            {
                Code = "VanDonId",
                Name = "Mã vận đơn"
            });

            exportedFields.Add(new BaseDto()
            {
                Code = "DauKyThay",
                Name = "Dấu ký thay"
            });
            exportedFields.Add(new BaseDto()
            {
                Code = "NgayKyNhan",
                Name = "Thời gian ký"
            });

            exportedFields.Add(new BaseDto()
            {
                Code = "TenNguoiKy",
                Name = "Tên người ký "
            });
            exportedFields.Add(new BaseDto()
            {
                Code = "NhanVienPhat",
                Name = "Nhân viên phát"
            });
            exportedFields.Add(new BaseDto()
            {
                Code = "BuuCucId",
                Name = "Bưu cục"
            });

            exportedFields.Add(new BaseDto()
            {
                Code = "Description",
                Name = "Ghi chú"
            });

            var request = new ManageKyNhanPagingRequest()
            {
                TextSearch = string.Empty,
                PageIndex = null,
                PageSize = 0
            };

            foreach (BaseDto item in exportedFields)
            {
                dc = new System.Data.DataColumn(item.Name);
                dt.Columns.Add(dc);
            }

            var data = await _kyNhanApiClient.GetManageListPaging(request);

            if (data != null)
            {
                columnsToTake = new List<string>();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    columnsToTake.Add(dt.Columns[i].ColumnName);
                }

                if (data.Items.Count() > 0)
                {
                    foreach (var item in data.Items)
                    {

                        System.Data.DataRow dataRow = dt.NewRow();
                        foreach (BaseDto exportedField in exportedFields)
                        {
                            propertyInfo = typeof(KyNhanDto).GetProperty(exportedField.Code);
                            dataRow[exportedField.Name] = propertyInfo.GetValue(item, null);
                        }

                        dt.Rows.Add(dataRow);
                    }
                }
                else
                {
                    System.Data.DataRow dataRow = dt.NewRow();
                    dt.Rows.Add(dataRow);
                }

                try
                {
                    byte[] fileBytes = ExcelExportHelper.ExportExcel(dt, "Danh sách ký nhận", false, columnsToTake.ToArray());

                    if (fileBytes == null || !fileBytes.Any())
                    {
                        throw new Exception(String.Format("Không có dữ liệu."));
                    }


                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "KyNhan.xlsx");
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("ERROR: " + ex.Message));
                }
            }
            else
            {
                throw new Exception(String.Format("NOT FOUND"));
            }
        }
    }
}
