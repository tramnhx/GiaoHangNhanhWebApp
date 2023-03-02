using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public List<T> Items { set; get; }
    }
}