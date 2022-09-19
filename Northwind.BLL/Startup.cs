using tools.Services;
using tools.WorkerPattern;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Northwind.BLL.Models;
using Northwind.BLL.Services;
using Northwind.BLL.Workers;
using Northwind.DAL.Models;

namespace Northwind.BLL
{
    /// <summary>
    /// Startup for the business logic layer. This also sets up entity framework because BLL depends on it.
    /// </summary>
    public static class Startup
    {
        public static IdentityService IdentityService
        {
            get
            {
                using (IServiceScope serviceScope = DAL.Startup.ApplicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    IdentityService service = serviceScope.ServiceProvider.GetRequiredService<IdentityService>();
                    return service;
                }
            }
        }

        public static void ConfigureServices(AppConfigurationModel appConfiguration, IServiceCollection services, IHostingEnvironment environment)
        {
            // Custom services
            // The configuration is split into 2 parts. 
            // - AppConfigurationService has the strongly typed configuration and filesystem.
            // - AppConfiguration is the strongly typed configuration only.
            services.AddSingleton<IAppConfigurationService>(new Northwind.BLL.Services.AppConfigurationService(environment));
            services.AddSingleton<IAppConfiguration>(new Northwind.BLL.Services.AppConfigurationService(environment));

            // Application Data - this is a dependant of business logic so we start it here
            Northwind.DAL.Startup.ConfigureServices(appConfiguration, services);
            
            // AspNetCore Identity
            Northwind.DAL.Startup.ConfigureIdentityServices(appConfiguration, services);
            services.AddSingleton<IdentityService, IdentityService>();

            services.AddTransient<RetailInventoryService, RetailInventoryService>();
            services.AddTransient<CategoryService, CategoryService>();
            services.AddTransient<IGenericWorker<Product, ProductRowApiO, int>, ProductRowWorker>();
            services.AddTransient<IGenericWorker<Customer, CustomerRowApiO, string>, CustomerRowWorker>();
            services.AddTransient<IGenericWorker<CustomerDemographic, CustomerDemographicRowApiO, string>, CustomerDemographicRowWorker>();
            services.AddTransient<IGenericWorker<Employee, EmployeeRowApiO, int>, EmployeeRowWorker>();
            services.AddTransient<IGenericWorker<EmployeeTerritory, EmployeeTerritoryRowApiO, int>, EmployeeTerritoryRowWorker>();
            services.AddTransient<IGenericWorker<Order, OrderRowApiO, int>, OrderRowWorker>();
            services.AddTransient<IGenericWorker<OrderDetail, OrderDetailRowApiO, int>, OrderDetailRowWorker>();
            services.AddTransient<IGenericWorker<Region, RegionRowApiO, int>, RegionRowWorker>();
            services.AddTransient<IGenericWorker<Shipper, ShipperRowApiO, int>, ShipperRowWorker>();
            services.AddTransient<IGenericWorker<Supplier, SupplierRowApiO, int>, SupplierRowWorker>();
            services.AddTransient<IGenericWorker<Territory, TerritoryRowApiO, int>, TerritoryRowWorker>();
        }

        public static void Configure(AppConfigurationModel appConfiguration, IApplicationBuilder app)
        {
            // Application Data
            Northwind.DAL.Startup.Configure(app);
            
            // AspNetCore Identity
            Northwind.DAL.Startup.ConfigureIdentity(appConfiguration, app);
        }
    }
}
