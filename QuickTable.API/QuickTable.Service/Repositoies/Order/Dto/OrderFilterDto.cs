using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;

namespace QuickTable.Service.Repositoies.Order.Dto
{
    public class OrderFilterDto : BaseQueryFilter
    {
        public int TableSessionId { get; set; }
        public string? Status { get; set; }
    }
}
