namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Users
{
    public class ChangePasswordRequest
    {
        public string UserName { set; get; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
