using duncans.WorkerPattern;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Northwind.BLL.Models;
using Northwind.BLL.Services;
using Northwind.DAL.Models;

namespace Northwind.BLL
{
    public static class Startup
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            Northwind.DAL.Startup.ConfigureServices(configuration, services);

            // Add the services we created here.
            services.AddScoped<IGenericService<CategoryDbModel, CategoryApiModel, int>, CategoriesService>();
            services.AddScoped<IGenericService<ProductDbModel, ProductApiModel, int>, ProductsService>();
        }

        public static void Configure(IApplicationBuilder app)
        {
            Northwind.DAL.Startup.Configure(app);
        }
    }
}
