using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiaoHangNhanh.DAL.Entities.EntityDto.Catalog.Genders
{
    public class GenderRequest
    {
        public int? Id { get; set; }
        public string Code { set; get; }
        public string Name { set; get; }
        public int LanguageId { set; get; }
        public Guid CreatedUserId { set; get; }
        public Guid ModifiedUserId { set; get; }
    }
}
