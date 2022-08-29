using Northwind.BLL.Services;
using Northwind.DAL;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

AppConfigurationService appConfig = AppConfigurationServiceSingleton.Create(builder.Environment);

Startup.ConfigureIdentityServices(appConfig.AppConfiguration, builder.Services, usingDefaultPages: true);
Startup.ConfigureSpaIdentityServices(builder.Services);

if (appConfig.IsDevelopment)
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddMvc(o => o.EnableEndpointRouting = false);

// |-------- END OF SERVICE CONFIG --------|

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// TODO - the startup seed data does not work?!
// The old logins work with the previous database. Maybe some keys need to be copied.
// In worst case - use an api to do things with identity. Implement here as client.
// Is there an identity server step which needs to be carried out?
// Also add the protectpersonaldata option.

Startup.ConfigureIdentity(appConfig.AppConfiguration, app, true);
Startup.ConfigureSpaIdentity(app);

app.UseMvc(routes =>
{
    routes.MapRoute(
            name: "areas",
            template: "{area:exists}/{controller=Home}/{action=Index}");

    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");    
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapFallbackToFile("index.html"); ;

app.Run();
