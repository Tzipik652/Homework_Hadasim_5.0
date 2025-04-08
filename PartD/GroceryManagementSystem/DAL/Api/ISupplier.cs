using System;
using System.Collections.Generic;
using DAL.Models;
namespace DAL.Api;

public interface ISupplier:ICrud<Supplier>
{
    Task<Supplier> GetByIdAsync(int supplierId);
}
