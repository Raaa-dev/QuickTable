using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies.Order.Dto
{
    public class OrderUpdateDto
    {
        public int TableSessionId { get; set; }

        public decimal? TotalAmount { get; set; }
    }
}
