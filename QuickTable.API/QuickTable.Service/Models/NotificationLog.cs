using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class NotificationLog
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string? Message { get; set; }

    public DateTime? SendAt { get; set; }

    public string? Status { get; set; }

    public virtual Order Order { get; set; } = null!;
}
