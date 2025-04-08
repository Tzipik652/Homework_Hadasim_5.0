using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class BLProduct
{
    public int ProductId { get; set; }

    public int? SupplierId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal? ItemPrice { get; set; }

    public int? MinimumPurchaseQuantity { get; set; }

}
