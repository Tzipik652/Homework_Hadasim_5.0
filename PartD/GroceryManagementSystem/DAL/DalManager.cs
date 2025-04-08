using DAL.Api;
using DAL.Models;
using DAL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace DAL;

public class DalManager : IDal
{

    public ISupplier Supplier { get; }
    public IProduct Product { get; }
    public IOrder Order { get; }
    public IUser User { get; }
    public IManager Manager { get; }
    public IStatus Status { get; }

    public DalManager()
    {
       
        ServiceCollection serCollection = new ServiceCollection();
        serCollection.AddDbContext<GroceryDbContext>(options =>
             options.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\tzipi\\Desktop\\הגשת מבחן בית-הדסים 5.0\\PartD\\GroceryManagementSystem\\DAL\\Data\\Grocery_store_management.mdf\";Integrated Security=True;Connect Timeout=30")); 

        serCollection.AddSingleton<GroceryDbContext>();
        serCollection.AddScoped<ISupplier, SupplierService>();
        serCollection.AddScoped<IProduct, ProductService>();
        serCollection.AddScoped<IOrder, OrderService>();
        serCollection.AddScoped<IUser, UserService>();
        serCollection.AddScoped<IManager, ManagerService>();
        serCollection.AddScoped<IStatus, StatusService>();
       

       
        ServiceProvider provider = serCollection.BuildServiceProvider();


        Supplier = provider.GetRequiredService<ISupplier>();
        Product = provider.GetRequiredService<IProduct>();
        Order = provider.GetRequiredService<IOrder>();
        User = provider.GetRequiredService<IUser>();
        Manager = provider.GetRequiredService<IManager>();
        Status = provider.GetRequiredService<IStatus>();
       


    }
}

