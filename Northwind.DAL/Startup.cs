using tools.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DAL.Models;
using Northwind.DAL.Models.Authentication;
using Northwind.DAL.Repositories;

namespace Northwind.DAL
{
    /// <summary>
    /// Public static methods to setup services and configure the application and identity database
    /// </summary>
    public static class Startup
    {
        public static AppConfigurationModel AppConfiguration { get; set; }

        public static IApplicationBuilder ApplicationBuilder { get; set; }

        public static NorthwindContext NorthwindContext
        {
            get
            {
                NorthwindContext result = null;

                DbContextOptionsBuilder<NorthwindContext> optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>()
                                                                                .UseSqlServer(Startup.AppConfiguration.ConnectionStrings.Content);
                result = new NorthwindContext(optionsBuilder.Options);

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

            services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(appConfiguration.ConnectionStrings.Content)); // Application data

            services.AddTransient<IRepository<Category, int>, CategoryRepository>();
            services.AddTransient<IRepository<CustomerDemographic, string>, CustomerDemographicRepository>();
            services.AddTransient<IRepository<CustomerCustomerDemo, string>, CustomerCustomerDemoRepository>();
            services.AddTransient<IRepository<Customer, string>, CustomerRepository>();
            services.AddTransient<IRepository<Employee, int>, EmployeeRepository>();
            services.AddTransient<IRepository<EmployeeTerritory, int>, EmployeeTerritoryRepository>();
            services.AddTransient<IRepository<OrderDetail, int>, OrderDetailRepository>();
            services.AddTransient<IRepository<Order, int>, OrderRepository>();
            services.AddTransient<IRepository<Product, int>, ProductRepository>();
            services.AddTransient<IRepository<Region, int>, RegionRepository>();
            services.AddTransient<IRepository<Shipper, int>, ShipperRepository>();
            services.AddTransient<IRepository<Supplier, int>, SupplierRepository>();
            services.AddTransient<IRepository<Territory, string>, TerritoryRepository>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            ApplicationBuilder = app;

            // Create and update the database automatically (like doing Update-Database)
            // https://stackoverflow.com/questions/42355481/auto-create-database-in-entity-framework-core
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                NorthwindContext applicationDbContext = serviceScope.ServiceProvider.GetRequiredService<NorthwindContext>();
                applicationDbContext.Database.Migrate();
            }

            SeedData.EnsurePopulated(app.ApplicationServices);
        }

        public static void ConfigureIdentityServices(AppConfigurationModel appConfiguration, IServiceCollection services)
        {
            services.AddDbContext<AuthenticationDbContext>(options => options.UseSqlServer(appConfiguration.ConnectionStrings.Authentication)); // The user database

            services.AddSingleton<IRepository<ApiSessionModel, string>, ApiLoginRepository>(); // The api logins repository

            // Setup the password validation requirements eg: Password1!
            services.AddIdentity<UserProfileModel, IdentityRole>(
                    opts =>
                    {
                        opts.User.RequireUniqueEmail = true;

                        opts.Password.RequiredLength = 9;
                        opts.Password.RequireNonAlphanumeric = false;
                        opts.Password.RequireLowercase = true;
                        opts.Password.RequireUppercase = true;
                        opts.Password.RequireDigit = true;
                    }
                ).AddEntityFrameworkStores<AuthenticationDbContext>(); // The model
        }

        public static void ConfigureIdentity(AppConfigurationModel appConfiguration, IApplicationBuilder app)
        {
            // Create and update the database automatically (like doing Update-Database)
            // https://stackoverflow.com/questions/42355481/auto-create-database-in-entity-framework-core
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                AuthenticationDbContext authenticationDbContext = serviceScope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
                authenticationDbContext.Database.Migrate();
            }

            // Seed data
            AuthenticationDbContext.CreateDefaultRoles(app.ApplicationServices, appConfiguration).Wait();
            AuthenticationDbContext.CreateAdminAccount(app.ApplicationServices, appConfiguration).Wait();

            app.UseAuthentication();
        }
    }
}
