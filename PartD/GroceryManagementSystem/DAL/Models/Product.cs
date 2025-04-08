using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public int? SupplierId { get; set; }

    public string? ProductName { get; set; }

    public decimal? ItemPrice { get; set; }

    public int? MinimumPurchaseQuantity { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Supplier? Supplier { get; set; }
}
