using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
