using System;
using System.Collections.Generic;

using DAL.Models;
using DAL.Api;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services
{
    public class UserService : IUser
    {

        GroceryDbContext _dataManager;
        public UserService(GroceryDbContext? m)
        {
            _dataManager = m ?? throw new ArgumentNullException(nameof(m), "Database context cannot be null.");
        }

        public async Task CreateAsync(User User)
        {
            if (await _dataManager.Users.AnyAsync(d => d.UserId == User.UserId))
                throw new Exception($"User with ID {User.UserId} already exists.");
            await _dataManager.Users.AddAsync(User);
            await _dataManager.SaveChangesAsync();
        }
        public async Task UpdateAsync(User User)
        {
            var UserToUpdate = await _dataManager.Users.FindAsync(User.UserId);
            if (UserToUpdate == null)
                throw new Exception($"User with ID {User.UserId} does not exist.");
            UserToUpdate.PasswordHash = User.PasswordHash;
      

            await _dataManager.SaveChangesAsync();
        }
        public async Task DeleteAsync(User User)
        {
            var UserToDelete = await _dataManager.Users.FindAsync(User.UserId);
            if (UserToDelete == null)
                throw new Exception($"User with ID {User.UserId} does not exist.");
            _dataManager.Users.Remove(UserToDelete);
            await _dataManager.SaveChangesAsync();
        }

        public Task<List<User>> GetAsync()
        {
            return _dataManager.Users.ToListAsync();
        }
        public async Task<User> GetByIdAsync(int UserId)
        {
            var UserById = await _dataManager.Users.FindAsync(UserId);
            if (UserById == null)
                throw new Exception($"User with ID {UserId} does not exist.");
            return UserById;
        }
      
    }

}
