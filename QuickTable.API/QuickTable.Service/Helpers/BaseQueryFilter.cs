using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Helpers
{
    public class BaseQueryFilter
    {
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 300;
    }
}
