namespace GiaoHangNhanh.DAL.Entities.EntityDto.Common
{
    public class AddEditRequest<T>
    {
        public int? Id { set; get; }
        public string Guid { set; get; }
        public T Data { set; get; }
    }
}