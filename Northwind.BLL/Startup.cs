using mezzanine.Services;
using mezzanine.WorkerPattern;
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
        public static void ConfigureServices(AppConfigurationModel appConfiguration, IServiceCollection services, IHostingEnvironment environment)
        {
            // Custom services
            services.AddSingleton<IAppConfigurationService>(new Northwind.BLL.Services.AppConfigurationService(environment)); // provides strongly typed access to appsettings.json.

            // Application Data - this is a dependant of business logic so we start it here
            Northwind.DAL.Startup.ConfigureServices(appConfiguration, services);
            
            // AspNetCore Identity
            Northwind.DAL.Startup.ConfigureIdentityServices(appConfiguration, services);
            services.AddTransient<IdentityService, IdentityService>();

            services.AddTransient<RetailInventoryService, RetailInventoryService>();
            services.AddTransient<CategoryService, CategoryService>();
            services.AddTransient<IGenericWorker<ProductRowApiO, int>, ProductWorker>();
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
