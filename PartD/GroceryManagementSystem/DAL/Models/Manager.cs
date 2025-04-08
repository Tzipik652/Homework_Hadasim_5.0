using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Manager
{
    public int ManagerId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
}
