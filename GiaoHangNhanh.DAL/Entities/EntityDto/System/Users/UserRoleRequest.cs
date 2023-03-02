namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Users
{
    public class UserRoleRequest
    {
        public int? LanguageId { set; get; }
        public string UserId { set; get; }
        public string ControllerName { set; get; }
        public string ActionName { set; get; }
    }
}
