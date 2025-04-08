using System;
using System.Collections.Generic;

using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;
using BL.Api;
using BL.Models;
using DAL.Services;
namespace BL.Services
{
    public class BLOrderService : IBLOrder
    {
        private readonly IDal dal;


        public BLOrderService(IDal d)
        {
            dal = d;
        }

    

        public async Task ValidateAsync(BLOrder order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order), "order cannot be null.");
    
            if (order.OrderDate > DateTime.Now)
                throw new ArgumentException("Order date cannot be in the future.");

        }
        public async Task CreateAsync(BLOrder order)
        {
            await ValidateAsync(order);
           
            await dal.Order.CreateAsync(await GetOrderAsync(order));
        }
        private async Task<Order> GetOrderAsync(BLOrder order)
        {
            
            return new Order
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                ProductId = order.ProductId,  
                Quantity = order.Quantity,
                StatusId = order.StatusId, 
            };
        }
        
        public async Task UpdateAsync(BLOrder order)
        {
            await ValidateAsync(order);
           
            await dal.Order.UpdateAsync(await GetOrderAsync(order));
        }
        public async Task DeleteAsync(BLOrder order)
        {
            await ValidateAsync(order);
           
            await dal.Order.DeleteAsync(await GetOrderAsync(order));
        }

        public async Task<List<BLOrder>> GetAsync()
        {
           
            var orders = await dal.Order.GetAsync();
            var blOrdersTasks = orders.Select(async order =>
            {
                var product = await dal.Product.GetByIdAsync(order.ProductId);
                var status = await dal.Status.GetByIdAsync(order.StatusId);

                return new BLOrder
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    ProductId = product?.ProductId ?? -1,
                    ProductName = product?.ProductName ?? "Unknown",
                    Quantity = order.Quantity,
                    StatusId = status?.StatusId ?? -1,
                    StatusName = status?.StatusName ?? "Unknown",
                    TotalPrice = (decimal)(product != null ? product.ItemPrice * order.Quantity : 0)
                };
            });

            var blOrders = await Task.WhenAll(blOrdersTasks);
            return blOrders.ToList();

        }

     
        public async Task<List<BLOrder>> GetOrdersBySupplierIdAsync(int supplierId)
        {
            var orders = await dal.Order.GetOrdersBySupplierIdAsync(supplierId);

            var blOrdersTasks = orders.Select(async order =>
            {
                var product = await dal.Product.GetByIdAsync(order.ProductId);
                var status = await dal.Status.GetByIdAsync(order.StatusId);

                return new BLOrder
                {
                    OrderId = order.OrderId,
                    OrderDate = order.OrderDate,
                    ProductId = product?.ProductId ?? -1,
                    ProductName = product?.ProductName ?? "Unknown",
                    Quantity = order.Quantity,
                    StatusId = status?.StatusId ?? -1,
                    StatusName = status?.StatusName ?? "Unknown",
                    TotalPrice = (decimal)(product != null ? product.ItemPrice * order.Quantity : 0)
                };
            });

            var blOrders = await Task.WhenAll(blOrdersTasks);
            return blOrders.ToList();
        }

        public async Task OrderingGoodsFromSupplierAsync(BLOrder order)
        {
            await ValidateAsync(order);

            var dalOrder = await GetOrderAsync(order);

            await dal.Order.CreateAsync(dalOrder);


        }

    }

}
