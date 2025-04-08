using System;
using System.Collections.Generic;

using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;
namespace DAL.Services
{
    public class StatusService : IStatus
    {

        GroceryDbContext _dataManager;
        public StatusService(GroceryDbContext? m)
        {
            _dataManager = m ?? throw new ArgumentNullException(nameof(m), "Database context cannot be null.");
        }
        public async Task CreateAsync(Status status)
        {
            if (string.IsNullOrWhiteSpace(status.StatusName))
                throw new ArgumentException("Status name cannot be empty.");
            if (await _dataManager.Statuses.AnyAsync(d => d.StatusId == status.StatusId))
                throw new Exception($"Status with ID {status.StatusId} already exists.");
            await _dataManager.Statuses.AddAsync(status);
            await _dataManager.SaveChangesAsync();
        }
        public async Task UpdateAsync(Status status)
        {
            if (string.IsNullOrWhiteSpace(status.StatusName))
                throw new ArgumentException("Status name cannot be empty.");
            var statusToUpdate = await _dataManager.Statuses.FindAsync(status.StatusId);
            if (statusToUpdate == null)
                throw new Exception($"status with ID {status.StatusId} does not exist.");
            statusToUpdate.StatusName = status.StatusName;
            await _dataManager.SaveChangesAsync();
        }
        public async Task DeleteAsync(Status status)
        {
            var statusToDelete = await _dataManager.Statuses.FindAsync(status.StatusId);
            if (statusToDelete == null)
                throw new Exception($"status with ID {status.StatusId} does not exist.");
            _dataManager.Statuses.Remove(statusToDelete);
            await _dataManager.SaveChangesAsync();
        }

        public Task<List<Status>> GetAsync()
        {
            return _dataManager.Statuses.ToListAsync();
        }
        public async Task<Status> GetByIdAsync(int? StatusId)
        {
            return await _dataManager.Statuses.FindAsync(StatusId);
        }
    

    }

}
