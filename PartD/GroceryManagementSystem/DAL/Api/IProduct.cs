using System;
using System.Collections.Generic;

using DAL.Models;
namespace DAL.Api;

public interface IProduct : ICrud<Product>
{
    Task<Product> GetByIdAsync(int? productId);
}
