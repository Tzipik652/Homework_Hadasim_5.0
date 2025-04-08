using System;
using System.Collections.Generic;

using BL.Models;
using BL.Api;
using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;
namespace BL.Services
{
    public class BLProductService : IBLProduct
    {

        private readonly IDal dal;
        public BLProductService(IDal d)
        {
            dal = d;
        }
        public async Task ValidateAsync(BLProduct Product)
        {
            if (string.IsNullOrWhiteSpace(Product.ProductName))
                throw new ArgumentException("Product name cannot be empty.");

            if (Product.ItemPrice <= 0)
                throw new ArgumentException("Item price must be greater than zero.");

            if (Product.MinimumPurchaseQuantity.HasValue && Product.MinimumPurchaseQuantity < 0)
                throw new ArgumentException("Minimum purchase quantity cannot be negative.");
          
        }
        public async Task CreateAsync(BLProduct Product)
        {
            await ValidateAsync(Product);
            Product Product_DAL = GetProduct(Product);
            await dal.Product.CreateAsync(Product_DAL);
        }
        private Product GetProduct(BLProduct Product) =>
           new Product
           {
               ProductId = Product.ProductId,
               SupplierId = Product.SupplierId,
               ProductName = Product.ProductName,
               ItemPrice = Product.ItemPrice,
               MinimumPurchaseQuantity = Product.MinimumPurchaseQuantity
           };
        public async Task UpdateAsync(BLProduct Product)
        {
            await ValidateAsync(Product);
            
            await dal.Product.UpdateAsync(GetProduct(Product));
        }
        public async Task DeleteAsync(BLProduct Product)
        {
            await ValidateAsync(Product);
           
            await dal.Product.DeleteAsync(GetProduct(Product));
        }

        public async Task<List<BLProduct>> GetAsync()
        {
            List<Product> list = await dal.Product.GetAsync();
            return list.Select(Product => new BLProduct()
            {
                ProductId = Product.ProductId,
                SupplierId = Product.SupplierId,
                ProductName = Product.ProductName,
                ItemPrice = Product.ItemPrice,
                MinimumPurchaseQuantity = Product.MinimumPurchaseQuantity
            }).ToList();
        }
        public async Task<BLProduct> GetByIdAsync(int ProductId)
        {
            Product Product = await dal.Product.GetByIdAsync(ProductId);
            return new BLProduct()
            {
                ProductId = Product.ProductId,
                SupplierId = Product.SupplierId,
                ProductName = Product.ProductName,
                ItemPrice = Product.ItemPrice,
                MinimumPurchaseQuantity = Product.MinimumPurchaseQuantity
            };
        }
        public async Task<List<BLProduct>> GetProductsBySupplierIdAsync(int supplierId)
        {
            List<Product> list = await dal.Product.GetAsync();

            return list
                .Where(product => product.SupplierId == supplierId) 
                .Select(product => new BLProduct()
                {
                    ProductId = product.ProductId,
                    SupplierId = product.SupplierId,
                    ProductName = product.ProductName,
                    ItemPrice = product.ItemPrice,
                    MinimumPurchaseQuantity = product.MinimumPurchaseQuantity
                }).ToList();
        }

    }

}
