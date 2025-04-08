using System;
using System.Collections.Generic;

namespace BL.Models;

public partial class BLManager
{
    public int ManagerId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

}
