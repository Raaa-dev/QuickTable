using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class MenuItem
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public bool? IsActive { get; set; }

    public virtual MenuCategory Category { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
