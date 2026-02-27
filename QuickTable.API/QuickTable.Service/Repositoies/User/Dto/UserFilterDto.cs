using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;

namespace QuickTable.Service.Repositoies.User.Dto
{
    public class UserFilterDto : BaseQueryFilter
    {
        public bool? IsActive { get; set; }
    }
}
