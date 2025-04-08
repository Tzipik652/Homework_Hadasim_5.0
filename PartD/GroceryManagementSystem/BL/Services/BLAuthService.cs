using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Api;
using BL.Models;
using DAL.Api;
using DAL.Models;

namespace BL.Services
{
    internal class BLAuthService:IBLAuth
    {
        private readonly IDal dal;
        public BLAuthService(IDal d)
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
        public async Task RegisterSupplierAsync(string username, string password, BLSupplier supplierDetails)
        {

            await ValidateAsync(supplierDetails);
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password are required");

            Supplier Supplier_DAL = GetSupplier(supplierDetails);
            await dal.Supplier.CreateAsync(Supplier_DAL);
            var user = new User
            {
                SupplierId = Supplier_DAL.SupplierId,
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await dal.User.CreateAsync(user);
        }
        private Supplier GetSupplier(BLSupplier supplier) =>
           new Supplier
           {
               //SupplierId = supplier.SupplierId,
               CompanyName = supplier.CompanyName,
               PhoneNumber = supplier.PhoneNumber,
               RepresentativeName = supplier.RepresentativeName
           };
        public async Task RegisterManagerAsync(string username, string password)
        {

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password are required");

            Manager manager = new Manager
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };
            await dal.Manager.CreateAsync(manager);
            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                SupplierId = manager.ManagerId
            };

            await dal.User.CreateAsync(user);
        }
      
        public async Task<(string Role, int Id)> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password are required");


            var users = await dal.User.GetAsync();
            var usersWithSameUsername = users.Where(u => u.Username == username).ToList();
            foreach (var user in usersWithSameUsername)
            {

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    if (user.SupplierId == null)
                    {
                        return ("MANAGER", user.UserId);
                    }
                    return ("SUPPLIER", user.SupplierId ?? 0);
                }
            }
            try
            {
                var manager = await dal.Manager.GetByNameAsync(username);
                if (manager != null && BCrypt.Net.BCrypt.Verify(password, manager.PasswordHash))
                {
                    return ("Manager", manager.ManagerId);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

            

            throw new UnauthorizedAccessException("Invalid username or password.");
        }

}
}
