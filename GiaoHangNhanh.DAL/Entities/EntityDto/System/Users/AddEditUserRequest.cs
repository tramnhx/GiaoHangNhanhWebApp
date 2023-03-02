namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.Users
{
    public class AddEditUserRequest<T>
    {
        public string Id { set; get; }
        public T Data { set; get; }
    }
}
