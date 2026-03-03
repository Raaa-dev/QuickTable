using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies.MenuItem.Dto
{
    public class MenuItemWriteDto
    {
        public int CategoryId { get; set; }

        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public bool? IsActive { get; set; }
    }
}
