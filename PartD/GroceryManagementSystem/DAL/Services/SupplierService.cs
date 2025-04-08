using System;
using System.Collections.Generic;

using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;
namespace DAL.Services
{
    public class SupplierService : ISupplier
    {

        GroceryDbContext _dataManager;
        public SupplierService(GroceryDbContext? m)
        {
            _dataManager = m ?? throw new ArgumentNullException(nameof(m), "Database context cannot be null.");
        }
        public async Task CreateAsync(Supplier supplier)
        {
            if (await _dataManager.Suppliers.AnyAsync(d => d.SupplierId == supplier.SupplierId))
                throw new Exception($"Supplier with ID {supplier.SupplierId} already exists.");
            await _dataManager.Suppliers.AddAsync(supplier);
            await _dataManager.SaveChangesAsync();
        }
        public async Task UpdateAsync(Supplier supplier)
        {
            var supplierToUpdate = await _dataManager.Suppliers.FindAsync(supplier.SupplierId);
            if (supplierToUpdate == null)
                throw new Exception($"Supplier with ID {supplier.SupplierId} does not exist.");
            supplierToUpdate.CompanyName = supplier.CompanyName;
            supplierToUpdate.PhoneNumber = supplier.PhoneNumber;
            supplierToUpdate.RepresentativeName = supplier.RepresentativeName;

            await _dataManager.SaveChangesAsync();
        }
        public async Task DeleteAsync(Supplier supplier)
        {
            var supplierToDelete = await _dataManager.Suppliers.FindAsync(supplier.SupplierId);
            if (supplierToDelete == null)
                throw new Exception($"Supplier with ID {supplier.SupplierId} does not exist.");
            _dataManager.Suppliers.Remove(supplierToDelete);
            await _dataManager.SaveChangesAsync();
        }

        public Task<List<Supplier>> GetAsync()
        {
            return _dataManager.Suppliers.ToListAsync();
        }
        public async Task<Supplier> GetByIdAsync(int supplierId)
        {
            var supplierById = await _dataManager.Suppliers.FindAsync(supplierId);
            if (supplierById == null)
                throw new Exception($"Supplier with ID {supplierId} does not exist.");
            return supplierById;
        }
    }

}
