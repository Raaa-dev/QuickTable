using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class MenuCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
