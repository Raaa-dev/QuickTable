using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies.Order.Dto
{
    public class OrderReadDto
    {
        public int Id { get; set; }

        public int TableSessionId { get; set; }

        public string? OrderNumber { get; set; }

        public decimal? TotalAmount { get; set; }

        public string? Status { get; set; }

        public List<OrderItemReadDto> Items { get; set; } = new List<OrderItemReadDto>();
    }
}
