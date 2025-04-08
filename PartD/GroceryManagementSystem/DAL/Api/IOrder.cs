using System;
using System.Collections.Generic;

using DAL.Models;
namespace DAL.Api;

public interface IOrder : ICrud<Order>
{
    Task<Order> GetByIdAsync(int OrderId);
    Task<List<Order>> GetOrdersByStatusAsync(int statusId);
    Task<List<Order>> GetOrdersBySupplierIdAsync(int supplierId);
}

