using System;
using System.Collections.Generic;

using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class ManagerService : IManager
    {

        GroceryDbContext _dataManager;
        public ManagerService(GroceryDbContext? m)
        {
            _dataManager = m ?? throw new ArgumentNullException(nameof(m), "Database context cannot be null.");
        }

        public async Task CreateAsync(Manager Manager)
        {
            if (await _dataManager.Managers.AnyAsync(d => d.ManagerId == Manager.ManagerId))
                throw new Exception($"Manager with ID {Manager.ManagerId} already exists.");
            await _dataManager.Managers.AddAsync(Manager);
            await _dataManager.SaveChangesAsync();
        }
        public async Task UpdateAsync(Manager Manager)
        {
            var ManagerToUpdate = await _dataManager.Managers.FindAsync(Manager.ManagerId);
            if (ManagerToUpdate == null)
                throw new Exception($"Manager with ID {Manager.ManagerId} does not exist.");
            ManagerToUpdate.Username = Manager.Username;
            ManagerToUpdate.PasswordHash = Manager.PasswordHash;

            await _dataManager.SaveChangesAsync();
        }
        public async Task DeleteAsync(Manager Manager)
        {
            var ManagerToDelete = await _dataManager.Managers.FindAsync(Manager.ManagerId);
            if (ManagerToDelete == null)
                throw new Exception($"Manager with ID {Manager.ManagerId} does not exist.");
            _dataManager.Managers.Remove(ManagerToDelete);
            await _dataManager.SaveChangesAsync();
        }

        public Task<List<Manager>> GetAsync()
        {
            return _dataManager.Managers.ToListAsync();
        }
        public async Task<Manager> GetByIdAsync(int ManagerId)
        {
            var ManagerById = await _dataManager.Managers.FindAsync(ManagerId);
            if (ManagerById == null)
                throw new Exception($"Manager with ID {ManagerId} does not exist.");
            return ManagerById;
        }
      
        public async Task<Manager> GetByNameAsync(string userName)
        {
            var user = await _dataManager.Managers
                   .FirstOrDefaultAsync(u => u.Username.ToLower() == userName.ToLower()); 
            if (user == null)
                throw new Exception($"Manager with name {userName} does not exist.");
            return user;
        }

    }

}
