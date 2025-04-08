using System;
using System.Collections.Generic;
using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;
namespace DAL.Services
{
    public class OrderService : IOrder
    {

        GroceryDbContext _dataManager;
        public OrderService(GroceryDbContext? m)
        {
            _dataManager = m ?? throw new ArgumentNullException(nameof(m), "Database context cannot be null.");
        }
        public async Task CreateAsync(Order order)
        {

            if (await _dataManager.Orders.AnyAsync(d => d.OrderId == order.OrderId))
                throw new Exception($"Order with ID {order.OrderId} already exists.");
            await _dataManager.Orders.AddAsync(order);
            await _dataManager.SaveChangesAsync();
        }

        public async Task UpdateAsync(Order order)
        {
            var orderToUpdate = await _dataManager.Orders.FindAsync(order.OrderId);
            if (orderToUpdate == null)
                throw new Exception($"Order with ID {order.OrderId} does not exist.");
            orderToUpdate.OrderDate = order.OrderDate;
            orderToUpdate.StatusId = order.StatusId;

            await _dataManager.SaveChangesAsync();
        }
        public async Task DeleteAsync(Order order)
        {
            var orderToDelete = await _dataManager.Orders.FindAsync(order.OrderId);
            if (orderToDelete == null)
                throw new Exception($"order with ID {order.OrderId} does not exist.");
            _dataManager.Orders.Remove(orderToDelete);
            await _dataManager.SaveChangesAsync();
        }

        public Task<List<Order>> GetAsync()
        {
            return _dataManager.Orders.ToListAsync();
        }
        public async Task<Order> GetByIdAsync(int orderId)
        {
            var orderById = await _dataManager.Orders.FindAsync(orderId);
            if (orderById == null)
                throw new Exception($"Order with ID {orderId} does not exist.");
            return orderById;
        }
        public async Task<List<Order>> GetOrdersByStatusAsync(int statusId)
        {
            return await _dataManager.Orders
                .Where(o => o.StatusId == statusId).Include(o => o.Product)
                .ToListAsync();
        }
        public async Task<List<Order>> GetOrdersBySupplierIdAsync(int supplierId)
        {
            var orders = await _dataManager.Orders
                .Join(_dataManager.Products, o => o.ProductId, p => p.ProductId, (o, p) => new { o, p })
                .Join(_dataManager.Statuses, op => op.o.StatusId, s => s.StatusId, (op, s) => new { op.o, op.p, s })
                .Where(x => x.p.SupplierId == supplierId)
                .Select(x => new Order
                {
                    OrderId = x.o.OrderId,
                    OrderDate = x.o.OrderDate,
                    ProductId = x.p.ProductId,
                    Quantity = x.o.Quantity,
                    StatusId = x.s.StatusId
                })
                .ToListAsync();

            return orders;
        }
    }
       
}
