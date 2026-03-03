using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies.Order.Dto
{
    public class OrderItemWriteDto
    {
        public int MenuItemId { get; set; }

        public decimal? Quantity { get; set; }
    }
}
