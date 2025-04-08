using BL.Api;
using BL.Services;
using DAL;
using DAL.Api;
using Microsoft.Extensions.DependencyInjection;

namespace BL
{
    public class BlManager : IBL
    {
        public IBLOrder Order { get; }
        public IBLProduct Products { get; }
        public IBLSupplier Supplier { get; }
        public IBLAuth Auth { get; }
        public IBLManager Manager { get; }

        public BlManager()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IDal, DalManager>();
            services.AddScoped<IBLOrder, BLOrderService>();
            services.AddScoped<IBLProduct, BLProductService>();
            services.AddScoped<IBLSupplier, BLSupplierService>();
            services.AddScoped<IBLAuth, BLAuthService>();
            services.AddScoped<IBLManager, BLManagerService>();

            ServiceProvider provider = services.BuildServiceProvider();

            Order = provider.GetRequiredService<IBLOrder>();
            Products = provider.GetRequiredService<IBLProduct>();
            Supplier = provider.GetRequiredService<IBLSupplier>();
            Auth = provider.GetRequiredService<IBLAuth>();
            Manager = provider.GetRequiredService<IBLManager>();
        }


    }
}
