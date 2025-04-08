using System;
using System.Collections.Generic;
using DAL.Models;
namespace DAL.Api;

public interface IStatus : ICrud<Status>
{
    Task<Status> GetByIdAsync(int? statusId);
}
