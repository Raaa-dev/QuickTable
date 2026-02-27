using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;

namespace QuickTable.Service.Repositoies.Table.Dto
{
    public class TableFilterDto : BaseQueryFilter
    {
        public bool? IsActive { get; set; }
    }
}
