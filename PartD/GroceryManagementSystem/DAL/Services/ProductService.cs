using System;
using System.Collections.Generic;

using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;
namespace DAL.Services
{
    public class ProductService : IProduct
    {

        GroceryDbContext _dataManager;
        public ProductService(GroceryDbContext? m)
        {
            _dataManager = m ?? throw new ArgumentNullException(nameof(m), "Database context cannot be null.");
        }
        public async Task CreateAsync(Product product)
        {
            if (await _dataManager.Products.AnyAsync(d => d.ProductId == product.ProductId))
                throw new Exception($"product with ID {product.ProductId} already exists.");
            await _dataManager.Products.AddAsync(product);
            await _dataManager.SaveChangesAsync();
        }
        public async Task UpdateAsync(Product product)
        {
            var productToUpdate = await _dataManager.Products.FindAsync(product.ProductId);
            if (productToUpdate == null)
                throw new Exception($"Product with ID {product.ProductId} does not exist.");
            productToUpdate.SupplierId = product.SupplierId;
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.ItemPrice = product.ItemPrice;
            productToUpdate.MinimumPurchaseQuantity = product.MinimumPurchaseQuantity;
            await _dataManager.SaveChangesAsync();
        }
        public async Task DeleteAsync(Product product)
        {
            var productToDelete = await _dataManager.Products.FindAsync(product.ProductId);
            if (productToDelete == null)
                throw new Exception($"Product with ID {product.ProductId} does not exist.");
            _dataManager.Products.Remove(productToDelete);
            await _dataManager.SaveChangesAsync();
        }

        public Task<List<Product>> GetAsync()
        {
            return _dataManager.Products.ToListAsync();
        }
        public async Task<Product> GetByIdAsync(int? productId)
        {
            var productById = await _dataManager.Products.FindAsync(productId);
            if (productById == null)
                throw new Exception($"Product with ID {productId} does not exist.");
            return productById;
        }

        
    }

}
