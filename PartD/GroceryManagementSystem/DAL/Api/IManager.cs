using System;
using System.Collections.Generic;
using DAL.Models;
namespace DAL.Api;

public interface IManager : ICrud<Manager>
{
    Task<Manager> GetByIdAsync(int managerId);
    Task<Manager> GetByNameAsync(string userName);
}
