using System;
using System.Collections.Generic;
using BL.Models;
namespace BL.Api;

public interface IBLSupplier
{
    Task<BLSupplier> GetByIdAsync(int SupplierId);
    Task OrderConfirmation(int orderId);

}
