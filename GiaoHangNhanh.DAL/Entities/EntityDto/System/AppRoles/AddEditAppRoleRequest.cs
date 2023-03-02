namespace GiaoHangNhanh.DAL.Entities.EntityDto.System.AppRoles
{
    public class AddEditAppRoleRequest<T>
    {
        public string Id { set; get; }
        public T Data { set; get; }
    }
}
