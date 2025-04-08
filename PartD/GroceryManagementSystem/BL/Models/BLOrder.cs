using System;
using System.Collections.Generic;
using DAL.Models;

namespace BL.Models;

public class BLOrder
{
    public int OrderId { get; set; }
    public DateTime? OrderDate { get; set; }
    public int? ProductId { get; set; }
    public string? ProductName { get; set; }
    public int? Quantity { get; set; }
    public int? StatusId { get; set; }
    public string? StatusName { get; set; }

    public decimal TotalPrice { get; set; }
}
