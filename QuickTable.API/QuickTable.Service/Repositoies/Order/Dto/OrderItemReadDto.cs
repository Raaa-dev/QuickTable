using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Repositoies.MenuItem.Dto;

namespace QuickTable.Service.Repositoies.Order.Dto
{
    public class OrderItemReadDto
    {
        public int MenuItemId { get; set; }
        public MenuItemReadDto? MenuItem { get; set; }
        public int OrderId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
    }
}
