using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Supplier
{
    public int? SupplierId { get; set; }

    public string? RepresentativeName { get; set; }

    public string? CompanyName { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
