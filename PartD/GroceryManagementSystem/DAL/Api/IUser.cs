using System;
using System.Collections.Generic;
using DAL.Models;
namespace DAL.Api;

public interface IUser : ICrud<User>
{
    Task<User> GetByIdAsync(int userId);
}
