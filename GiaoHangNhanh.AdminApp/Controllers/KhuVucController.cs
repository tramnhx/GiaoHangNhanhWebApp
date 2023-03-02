using BHSNetCoreLib.ExcelUtil;
using GiaoHangNhanh.AdminApp.Models;
using GiaoHangNhanh.ApiIntegration;
using GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.KhuVucs;
using GiaoHangNhanh.DAL.Entities.EntityDto.Common;
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
    public class KhuVucController : BaseController
    {
        private readonly IKhuVucApiClient _khuVucApiClient;
        public KhuVucController(IKhuVucApiClient khuVucApiClient)
        {
            _khuVucApiClient = khuVucApiClient;
        }
        public IActionResult Index()
        {
            KhuVucViewModel model = new KhuVucViewModel();
            model.CurrentUserRole = InternalService.FixedUserRole(HttpContext.Session.GetObject<UserDto>(SystemConstants.UserConstants.CurrentUserRoleSession),
                                                                                                            (ControllerContext.ActionDescriptor).ControllerName,
                                                                                                            (ControllerContext.ActionDescriptor).ActionName);

            model.PageTitle = "Khu Vực";
            model.Breadcrumbs = new List<string>() { "Quản trị hệ thống", "Danh mục hệ thống", "Khu Vực" };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DataTableGetList(int? draw, int? start, int? length, string textSearch,
                                                            string filterByTinhId, string filterByHuyenId)
        {
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDir = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = start != null ? Convert.ToInt32((start / length) + 1) : 1;
            int pageSize = length != null ? Convert.ToInt32(length) : 10;
            int filterByTinhIdValue, filterByHuyenIdValue;

            var request = new ManageKhuVucPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = skip,
                PageSize = pageSize,
                OrderCol = !string.IsNullOrEmpty(sortColumn) ? sortColumn : "Id",
                OrderDir = !string.IsNullOrEmpty(sortColumnDir) ? sortColumnDir : "desc",
                FilterByTinhId = int.TryParse(filterByTinhId, out filterByTinhIdValue) ? filterByTinhIdValue : new Nullable<int>(),
                FilterByHuyenId = int.TryParse(filterByHuyenId, out filterByHuyenIdValue) ? filterByHuyenIdValue : new Nullable<int>()
            };

            var khuVucApiClient = await _khuVucApiClient.GetManageListPaging(request);

            return Json(new
            {
                draw = draw,
                recordsFiltered = khuVucApiClient.TotalRecords,
                recordsTotal = khuVucApiClient.TotalRecords,
                data = khuVucApiClient.Items
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteByIds([FromBody] DeleteRequest request)
        {
            return Json(await _khuVucApiClient.DeleteByIds(request));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] AddEditRequest<KhuVucRequest> rq)
        {
            ApiResult<int> result = null;

            Guid userGuid = Guid.Parse(HttpContext.Session.GetString(SystemConstants.UserConstants.UserId));

            if (rq != null)
            {
                if (rq.Id == null)
                {
                    rq.Data.CreatedUserId = userGuid;
                    rq.Data.ModifiedUserId = userGuid;
                }
                else
                {
                    rq.Data.ModifiedUserId = userGuid;
                    rq.Data.Id = rq.Id.Value;
                }
                result = await _khuVucApiClient.AddOrUpdateAsync(rq.Data);
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


        public async Task<FileResult> ExportToExcel()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataColumn dc;
            List<string> columnsToTake;
            System.Reflection.PropertyInfo propertyInfo;
            List<BaseDto> exportedFields = new List<BaseDto>();

            exportedFields.Add(new BaseDto()
            {
                Code = "Code",
                Name = "Mã"
            });

            exportedFields.Add(new BaseDto()
            {
                Code = "Name",
                Name = "Tên"
            });


            exportedFields.Add(new BaseDto()
            {
                Code = "Description",
                Name = "Ghi chú"
            });

            var request = new ManageKhuVucPagingRequest()
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

            var data = await _khuVucApiClient.GetManageListPaging(request);

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
                            propertyInfo = typeof(KhuVucDto).GetProperty(exportedField.Code);
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
                    byte[] fileBytes = ExcelExportHelper.ExportExcel(dt, "Đơn vị tính", false, columnsToTake.ToArray());

                    if (fileBytes == null || !fileBytes.Any())
                    {
                        throw new Exception(String.Format("Không có dữ liệu."));
                    }


                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "DonViTinh.xlsx");
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

        [HttpGet]
        public async Task<IActionResult> GetAll(string textSearch)
        {
            var request = new ManageKhuVucPagingRequest()
            {
                TextSearch = textSearch,
            };
            var khuVucs = await _khuVucApiClient.GetAll(request);
            return Ok(khuVucs);
        }
        public async Task<IActionResult> Filter(string textSearch, int filterByHuyenId)
        {
            var request = new ManageKhuVucPagingRequest()
            {
                TextSearch = textSearch,
                PageIndex = 1,
                PageSize = 20,
                OrderCol = "Id",
                OrderDir = "desc",
                FilterByHuyenId = filterByHuyenId
            };

            var khuVucApiClient = await _khuVucApiClient.GetManageListPaging(request);
            return Ok(khuVucApiClient);
        }

    }
}
