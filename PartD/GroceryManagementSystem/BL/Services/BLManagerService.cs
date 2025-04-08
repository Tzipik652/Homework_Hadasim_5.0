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
    public class BLManagerService : IBLManager
    {

        private readonly IDal dal;
        public BLManagerService(IDal d)
        {
            dal = d;
        }
       
        public async Task OrderCompletion(int orderId)
        {
            var order = await dal.Order.GetByIdAsync(orderId);
            if (order == null)
                throw new Exception("Order not found");

            order.StatusId = 3;

            await dal.Order.UpdateAsync(order); 
        }
    }

}
