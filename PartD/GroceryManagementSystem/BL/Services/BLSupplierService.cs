using System;
using System.Collections.Generic;
using BCrypt.Net;

using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;
using BL.Models;
using BL.Api;
namespace BL.Services
{
    public class BLSupplierService : IBLSupplier
    {

        private readonly IDal dal;
        public BLSupplierService(IDal d)
        {
            dal = d;
        }
        public async Task ValidateAsync(BLSupplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.CompanyName))
                throw new ArgumentException("Company name cannot be empty.");

            if (string.IsNullOrWhiteSpace(supplier.PhoneNumber) || !System.Text.RegularExpressions.Regex.IsMatch(supplier.PhoneNumber, @"^\+?\d{9,15}$"))
                throw new ArgumentException("Invalid phone number format.");

            if (string.IsNullOrWhiteSpace(supplier.RepresentativeName))
                throw new ArgumentException("Representative name cannot be empty.");

        }
        public async Task CreateAsync(BLSupplier supplier, string username, string password)
        {
            await ValidateAsync(supplier);
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password are required");

            Supplier Supplier_DAL = GetSupplier(supplier);
            await dal.Supplier.CreateAsync(Supplier_DAL);
            var user = new User
            {
                SupplierId = supplier.SupplierId,
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await dal.User.CreateAsync(user);

        }
        private Supplier GetSupplier(BLSupplier supplier) =>
           new Supplier
           {
               SupplierId = supplier.SupplierId,
               CompanyName = supplier.CompanyName,
               PhoneNumber = supplier.PhoneNumber,
               RepresentativeName = supplier.RepresentativeName
           };
        public async Task UpdateAsync(BLSupplier Supplier)
        {
            await ValidateAsync(Supplier);
           
            await dal.Supplier.UpdateAsync(GetSupplier(Supplier));
        }
        public async Task DeleteAsync(BLSupplier Supplier)
        {
            await ValidateAsync(Supplier);
           
            await dal.Supplier.DeleteAsync(GetSupplier(Supplier));
        }

        public async Task<List<BLSupplier>> GetAsync()
        {
            List<Supplier> list = await dal.Supplier.GetAsync();
            return list.Select(supplier => new BLSupplier()
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                PhoneNumber = supplier.PhoneNumber,
                RepresentativeName = supplier.RepresentativeName
            }).ToList();
        }
        public async Task<BLSupplier> GetByIdAsync(int SupplierId)
        {
            Supplier supplier = await dal.Supplier.GetByIdAsync(SupplierId);
            return new BLSupplier()
            {
                SupplierId = supplier.SupplierId,
                CompanyName = supplier.CompanyName,
                PhoneNumber = supplier.PhoneNumber,
                RepresentativeName = supplier.RepresentativeName
            };
        }
        public async Task OrderConfirmation(int orderId)
        {
            var order = await dal.Order.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            order.StatusId = 2;

            await dal.Order.UpdateAsync(order); 
        }
    }

}
