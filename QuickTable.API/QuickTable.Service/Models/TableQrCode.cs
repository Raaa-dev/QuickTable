using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class TableQrCode
{
    public int Id { get; set; }

    public int TableId { get; set; }

    public string? QrToken { get; set; }

    public bool? IsActive { get; set; }

    public virtual Table Table { get; set; } = null!;
}
