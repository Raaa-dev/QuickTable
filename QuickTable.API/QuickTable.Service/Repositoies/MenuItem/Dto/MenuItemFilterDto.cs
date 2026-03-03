using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Helpers;

namespace QuickTable.Service.Repositoies.MenuItem.Dto
{
    public class MenuItemFilterDto : BaseQueryFilter
    {
        public int CategoryId { get; set; } 
        public bool? IsActive { get; set; }
    }
}
