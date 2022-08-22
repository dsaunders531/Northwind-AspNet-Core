using tools.Extensions;
using tools.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Northwind.DAL.Models;

namespace Northwind
{
    public class Startup
    {        
        private IHostingEnvironment Environment { get; set; } = null;
        private Northwind.BLL.Services.AppConfigurationService ConfigurationService { get; set; }
        private AppConfigurationModel AppConfiguration { get => ConfigurationService.AppConfiguration; }

        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
            ConfigurationService = new Northwind.BLL.Services.AppConfigurationService(environment);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string applicationName = Environment.ApplicationName.Replace(" ", string.Empty).Trim();

            // Built-in services
            services.AddMemoryCache();

            services.AddSession(options =>
                {
                    options.Cookie.Name = string.Format(".{0}.Session", applicationName);
                }
            );

            services.AddLocalization();

            services.AddAntiforgery(options =>
                {
                    options.Cookie.Name = string.Format(".{0}.AntiForgery", applicationName);
                    options.FormFieldName = "AntiForgery";
                    options.HeaderName = "x-application-anti-forgery";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Strict;
                options.ConsentCookie.Name = string.Format(".{0}.Consent", Environment.ApplicationName.Replace(" ", string.Empty).Trim());
            });

            // Business logic services - this also configures the database services & Identity services
            Northwind.BLL.Startup.ConfigureServices(AppConfiguration, services, Environment);

            services.AddMvc(o => o.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure the custom logger.
            ConfigureLogger(loggerFactory);

            // Business Logic Configuration - this also configures database services & Identity services
            Northwind.BLL.Startup.Configure(AppConfiguration, app);

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseStatusCodePages();

                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                //{
                //    HotModuleReplacement = true
                //});
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "areas",
                        template: "{area:exists}/{controller=Home}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                //routes.MapSpaFallbackRoute(
                //    name: "spa-fallback",
                //    defaults: new { controller = "Home", action = "Index" });
            });
        }

        private void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            //if (AppConfiguration.Logging.StdOutEnabled == true)
            //{
            //    loggerFactory.AddProvider(new ConsoleLoggerProvider());
            //}

            if (AppConfiguration.Logging.LogXMLEnabled == true)
            {
                loggerFactory.AddProvider(new XMLLoggerProvider(AppConfiguration.Logging.LogXMLLevel.ToLogLevel(),
                                                                ConfigurationService.WebRootPath + AppConfiguration.Logging.LogXMLPath,
                                                                AppConfiguration.Logging.LogRotateMaxEntries));
            }
        }
    }
}
