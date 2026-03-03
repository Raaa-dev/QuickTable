using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickTable.Service.Repositoies.Table.Dto;

namespace QuickTable.Service.Repositoies.TableSession.Dto
{
    public class TableResolveDto
    {
        public int TableId { get; set; }
        public string? Table { get; set; }

        public int SessionId { get; set; }
    }
}
