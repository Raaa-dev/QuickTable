using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies.Order.Dto
{
    public class CreateOrderRequestDto
    {
        //public int TableId { get; set; }

        public string QrToken { get; set; } = null!;

        // List of menu items the user selected
        public List<OrderItemWriteDto> Items { get; set; } = new List<OrderItemWriteDto>();
    }
}
