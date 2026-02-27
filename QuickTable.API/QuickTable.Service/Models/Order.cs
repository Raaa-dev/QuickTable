using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class Order
{
    public int Id { get; set; }

    public int TableSessionId { get; set; }

    public string? OrderNumber { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<NotificationLog> NotificationLogs { get; set; } = new List<NotificationLog>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual TableSession TableSession { get; set; } = null!;
}
