using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Northwind
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Note: UseDefaultServiceProvider has to be added to the builder.
        /// Otherwise app.ApplicationServices.GetRequiredService() does not work.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false);
    }
}
