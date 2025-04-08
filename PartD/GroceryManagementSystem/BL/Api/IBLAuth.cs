using System;
using System.Collections.Generic;

using BL.Models;
namespace BL.Api;

public interface IBLAuth
{
    Task RegisterSupplierAsync(string username, string password, BLSupplier supplierDetails);
    Task RegisterManagerAsync(string username, string password);
    Task<(string Role, int Id)> LoginAsync(string username, string password);
}
