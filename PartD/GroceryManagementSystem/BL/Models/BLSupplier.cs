using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class BLSupplier
{
    public int? SupplierId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string RepresentativeName { get; set; } = null!;

}
