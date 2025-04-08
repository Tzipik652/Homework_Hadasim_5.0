using System;
using System.Collections.Generic;

using BL.Models;
using DAL.Models;
namespace BL.Api;

public interface IBLOrder : IBLCrud<BLOrder>
{
    Task<List<BLOrder>> GetOrdersBySupplierIdAsync(int supplierId);
    Task OrderingGoodsFromSupplierAsync(BLOrder order);
}

