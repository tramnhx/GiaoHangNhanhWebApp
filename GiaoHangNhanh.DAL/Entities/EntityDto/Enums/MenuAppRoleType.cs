using System;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Enums
{
    public enum MenuAppRoleType : Byte
    {
        SystemDataView = 1,
        SystemDataEdit = 2,
        SystemDataDelete = 3,
        PersonalDataView = 4,
        PersonalDataEdit = 5,
        PersonalDataDelete = 6,
        DownloadExcel = 7
    }
}
