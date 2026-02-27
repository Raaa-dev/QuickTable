using System;
using System.Collections.Generic;

namespace QuickTable.Service.Models;

public partial class User
{
    public int Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? IsActive { get; set; }
}
