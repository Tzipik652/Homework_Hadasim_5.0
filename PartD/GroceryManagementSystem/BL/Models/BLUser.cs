using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class BLUser
{
    public int UserId { get; set; }

    public int? SupplierId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

}
