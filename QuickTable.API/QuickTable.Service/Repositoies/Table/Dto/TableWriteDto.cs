using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTable.Service.Repositoies.Table.Dto
{
    public class TableWriteDto
    {
        public string TableNumber { get; set; } = null!;

        public int? Capacity { get; set; }

        public bool? IsActive { get; set; }
    }
}
