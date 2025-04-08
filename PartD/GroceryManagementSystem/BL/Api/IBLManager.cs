using System;
using System.Collections.Generic;
using BL.Models;
namespace BL.Api;

public interface IBLManager
{
    Task OrderCompletion(int orderId);

}
