using duncans.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DAL.Models;
using Northwind.DAL.Repositories;
using Northwind.DAL.Services;

namespace Northwind.DAL
{
    /// <summary>
    /// Public static methods to setup services and configure the application and identity database
    /// </summary>
    public static class Startup
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))); // Application data

            services.AddScoped<IRepository<CategoryDbModel, int>, CategoryRepository>();
            services.AddScoped<IRepository<CustomerDemographicDbModel, long>, CustomerDemographicRepository>();
            services.AddScoped<IRepository<CustomerCustomerDemoDbModel, long>, CustomerCustomerDemoRepository>();
            services.AddScoped<IRepository<CustomerDbModel, int>, CustomerRepository>();
            services.AddScoped<IRepository<EmployeeDbModel, int>, EmployeeRepository>();
            services.AddScoped<IRepository<EmployeeTerritoryDbModel, int>, EmployeeTerritoryRepository>();
            services.AddScoped<IRepository<OrderDetailDbModel, int>, OrderDetailRepository>();
            services.AddScoped<IRepository<OrderDbModel, int>, OrderRepository>();
            services.AddScoped<IRepository<ProductDbModel, int>, ProductRepository>();
            services.AddScoped<IRepository<RegionDbModel, int>, RegionRepository>();
            services.AddScoped<IRepository<ShipperDbModel, int>, ShipperRepository>();
            services.AddScoped<IRepository<SupplierDbModel, int>, SupplierRepository>();
            services.AddScoped<IRepository<TerritoryDbModel, int>, TerritoryRepository>();
            services.AddScoped<IRepository<ProductHistoryDbModel, long>, ProductHistoryRepository>();
            services.AddScoped<IRepository<CustomerHistoryDbModel, long>, CustomerHistoryRepository>();
            services.AddScoped<IRepository<EmployeeHistoryDbModel, long>, EmployeeHistoryRepository>();

            // History Services
            services.AddScoped<IHistoryService<CustomerDbModel, int, CustomerHistoryDbModel>, CustomerHistoryService>();
            services.AddScoped<IHistoryService<ProductDbModel, int, ProductHistoryDbModel>, ProductHistoryService>();
            services.AddScoped<IHistoryService<EmployeeDbModel, int, EmployeeHistoryDbModel>, EmployeeHistoryService>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            // Create and update the database automatically (like doing Update-Database)
            // https://stackoverflow.com/questions/42355481/auto-create-database-in-entity-framework-core
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {                
                NorthwindDbContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<NorthwindDbContext>();
                applicationDbContext.Database.Migrate();
            }
        }
    }
}
