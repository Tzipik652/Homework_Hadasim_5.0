using System;
using System.Collections.Generic;

using BL.Models;
namespace BL.Api;

public interface IBLProduct : IBLCrud<BLProduct> {
    Task<BLProduct> GetByIdAsync(int ProductId);
    Task<List<BLProduct>> GetProductsBySupplierIdAsync(int supplierId);
}
