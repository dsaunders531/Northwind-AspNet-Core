using mezzanine.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DAL.Models;
using Northwind.DAL.Models.Authentication;
using Northwind.DAL.Repositories;
using Northwind.DAL.Services;

namespace Northwind.DAL
{
    /// <summary>
    /// Public static methods to setup services and configure the application and identity database
    /// </summary>
    public static class Startup
    {
        public static AppConfigurationModel AppConfiguration { get; set; }

        public static IApplicationBuilder ApplicationBuilder { get; set; }

        public static NorthwindDbContext NorthwindContext
        {
            get
            {
                NorthwindDbContext result = null;

                DbContextOptionsBuilder<NorthwindDbContext> optionsBuilder = new DbContextOptionsBuilder<NorthwindDbContext>()
                                                                                .UseSqlServer(Startup.AppConfiguration.ConnectionStrings.Content);
                result = new NorthwindDbContext(optionsBuilder.Options);

                optionsBuilder = null;

                return result;              
            }
        }

        public static IRepository<ApiSessionModel, string> ApiLoginRepository
        {
            get
            {
                using (IServiceScope serviceScope = ApplicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    IRepository<ApiSessionModel, string> service = serviceScope.ServiceProvider.GetRequiredService<IRepository<ApiSessionModel, string>>();
                    return service;
                }
            }
        }

        public static void ConfigureServices(AppConfigurationModel appConfiguration, IServiceCollection services)
        {
            Startup.AppConfiguration = appConfiguration;

            services.AddDbContext<NorthwindDbContext>(options => options.UseSqlServer(appConfiguration.ConnectionStrings.Content)); // Application data

            services.AddTransient<IRepository<CategoryDbModel, int>, CategoryRepository>();
            services.AddTransient<IRepository<CustomerDemographicDbModel, long>, CustomerDemographicRepository>();
            services.AddTransient<IRepository<CustomerCustomerDemoDbModel, long>, CustomerCustomerDemoRepository>();
            services.AddTransient<IRepository<CustomerDbModel, int>, CustomerRepository>();
            services.AddTransient<IRepository<EmployeeDbModel, int>, EmployeeRepository>();
            services.AddTransient<IRepository<EmployeeTerritoryDbModel, int>, EmployeeTerritoryRepository>();
            services.AddTransient<IRepository<OrderDetailDbModel, int>, OrderDetailRepository>();
            services.AddTransient<IRepository<OrderDbModel, int>, OrderRepository>();
            services.AddTransient<IRepository<ProductDbModel, int>, ProductRepository>();
            services.AddTransient<IRepository<RegionDbModel, int>, RegionRepository>();
            services.AddTransient<IRepository<ShipperDbModel, int>, ShipperRepository>();
            services.AddTransient<IRepository<SupplierDbModel, int>, SupplierRepository>();
            services.AddTransient<IRepository<TerritoryDbModel, int>, TerritoryRepository>();
            services.AddTransient<IRepository<ProductHistoryDbModel, long>, ProductHistoryRepository>();
            services.AddTransient<IRepository<CustomerHistoryDbModel, long>, CustomerHistoryRepository>();
            services.AddTransient<IRepository<EmployeeHistoryDbModel, long>, EmployeeHistoryRepository>();

            // History Services
            services.AddTransient<IHistoryService<CustomerDbModel, int, CustomerHistoryDbModel>, CustomerHistoryService>();
            services.AddTransient<IHistoryService<ProductDbModel, int, ProductHistoryDbModel>, ProductHistoryService>();
            services.AddTransient<IHistoryService<EmployeeDbModel, int, EmployeeHistoryDbModel>, EmployeeHistoryService>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            ApplicationBuilder = app;

            // Create and update the database automatically (like doing Update-Database)
            // https://stackoverflow.com/questions/42355481/auto-create-database-in-entity-framework-core
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {                
                NorthwindDbContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<NorthwindDbContext>();
                applicationDbContext.Database.Migrate();
            }

            SeedData.EnsurePopulated(app.ApplicationServices);
        }

        public static void ConfigureIdentityServices(AppConfigurationModel appConfiguration, IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(appConfiguration.ConnectionStrings.Identity)); // The user database

            services.AddSingleton<IRepository<ApiSessionModel, string>, ApiLoginRepository>(); // The api logins repository

            // Setup the password validation requirements eg: Password1!
            services.AddIdentity<IdentityUserModel, IdentityRoleModel>(
                    opts =>
                    {
                        opts.User.RequireUniqueEmail = true;

                        opts.Password.RequiredLength = 9;
                        opts.Password.RequireNonAlphanumeric = false;
                        opts.Password.RequireLowercase = true;
                        opts.Password.RequireUppercase = true;
                        opts.Password.RequireDigit = true;
                    }
                ).AddEntityFrameworkStores<IdentityDbContext>(); // The model
        }

        public static void ConfigureIdentity(AppConfigurationModel appConfiguration, IApplicationBuilder app)
        {
            // Create and update the database automatically (like doing Update-Database)
            // https://stackoverflow.com/questions/42355481/auto-create-database-in-entity-framework-core
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                IdentityDbContext authenticationDbContext = serviceScope.ServiceProvider.GetRequiredService<IdentityDbContext>();
                authenticationDbContext.Database.Migrate();
            }

            // Seed data
            IdentityDbContext.CreateDefaultRoles(app.ApplicationServices, appConfiguration).Wait();
            IdentityDbContext.CreateAdminAccount(app.ApplicationServices, appConfiguration).Wait();

            app.UseAuthentication();
        }
    }
}
