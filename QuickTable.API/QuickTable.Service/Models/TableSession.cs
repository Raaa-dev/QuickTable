using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class TableSession
{
    public int Id { get; set; }

    public int TableId { get; set; }

    public string? SessionCode { get; set; }

    public string? Status { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? EndAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Table Table { get; set; } = null!;
}
