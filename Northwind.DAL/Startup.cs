using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.DAL.Models;
using Northwind.DAL.Models.Authentication;
using Northwind.DAL.Repositories;
using tools.EF;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

        /// <summary>
        /// Configure Identity Services
        /// </summary>
        /// <param name="appConfiguration">Where the connection string is.</param>
        /// <param name="services">The service collection to put the identity services</param>
        /// <param name="usingDefaultPages">If you are implementing your own (false default) or are using the ones in the rcl (true).</param>
        public static void ConfigureIdentityServices(AppConfigurationModel appConfiguration, IServiceCollection services, bool usingDefaultPages = false)
        {            
            services.AddDbContext<AuthenticationDbContext>(options => options.UseSqlServer(appConfiguration.ConnectionStrings.Authentication)); // The user database
            
            services.AddSingleton<IRepository<ApiSessionModel, string>, ApiLoginRepository>(); // The api logins repository

            // TODO - add dataprotection option here and modify the authentication context.
            System.Action<IdentityOptions> options = opts => {
                opts.User.RequireUniqueEmail = true;

                opts.Password.RequiredLength = 9;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequireDigit = true;

                opts.SignIn.RequireConfirmedAccount = true;                
            };

            // Spa needs default identity for the routes etc.
            if (usingDefaultPages)
            {
                services.AddDefaultIdentity<UserProfileModel>(options)                    
                    .AddEntityFrameworkStores<AuthenticationDbContext>()
                    .AddRoles<IdentityRole>()
                    .AddRoleStore<RoleStore<IdentityRole, AuthenticationDbContext>>()
                    .AddUserStore<UserStore<UserProfileModel, IdentityRole, AuthenticationDbContext>>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddUserManager<UserManager<UserProfileModel>>();                                                                               
            }
            else
            {
                // Setup the password validation requirements eg: Password1!            
                services.AddIdentity<UserProfileModel, IdentityRole>(options)
                    .AddEntityFrameworkStores<AuthenticationDbContext>();                       
            }            
        }

        public static void ConfigureSpaIdentityServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddApiAuthorization<UserProfileModel, AuthenticationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
        }
        /// <summary>
        /// Configure Identity
        /// </summary>
        /// <param name="appConfiguration"></param>
        /// <param name="app"></param>
        /// <param name="createSeedUserAndRoles">Create the seed user and roles</param>
        /// <param name="resetAdminPassword">Reset admin password to default</param>
        public static void ConfigureIdentity(AppConfigurationModel appConfiguration, IApplicationBuilder app, bool createSeedUserAndRoles)
        {
            // Create and update the database automatically (like doing Update-Database)
            // https://stackoverflow.com/questions/42355481/auto-create-database-in-entity-framework-core
            using (IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                AuthenticationDbContext authenticationDbContext = serviceScope.ServiceProvider.GetRequiredService<AuthenticationDbContext>();
                authenticationDbContext.Database.Migrate();

                // Seed data
                if (createSeedUserAndRoles)
                {
                    AuthenticationDbContext.CreateDefaultRoles(serviceScope.ServiceProvider, appConfiguration).Wait();
                    AuthenticationDbContext.CreateAdminAccount(serviceScope.ServiceProvider, appConfiguration).Wait();
                }                
            }
                        
            app.UseAuthentication();
        }

        public static void ConfigureSpaIdentity(IApplicationBuilder app)
        {
            app.UseIdentityServer();
            app.UseAuthorization();
        }
    }
}
