using System;
using System.Collections.Generic;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Common
{
    public class DeleteRequest
    {
        public List<int> Ids { set; get; }
        public List<Guid> GuidIds { set; get; }
        public Guid DeleteUserId { get; set; }
        public int Id { get; set; }
    }
}
