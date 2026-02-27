using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class Table
{
    public int Id { get; set; }

    public string TableNumber { get; set; } = null!;

    public int? Capacity { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<TableQrCode> TableQrCodes { get; set; } = new List<TableQrCode>();

    public virtual ICollection<TableSession> TableSessions { get; set; } = new List<TableSession>();
}
