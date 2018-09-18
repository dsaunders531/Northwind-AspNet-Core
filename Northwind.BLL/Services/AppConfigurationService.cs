using mezzanine.Utility;
using Microsoft.AspNetCore.Hosting;
using Northwind.DAL.Models;
using System;

namespace Northwind.BLL.Services
{
    /// <summary>
    /// Use the mezzanine configuration service to provide a strongly typed configuration in the application.
    /// </summary>
    public sealed class AppConfigurationService: mezzanine.Services.AppConfigurationService
    {
        public AppConfigurationModel AppConfiguration { get; private set; }

        public AppConfigurationService(IHostingEnvironment env) : base(env)
        { }

        public override void LoadJsonConfig()
        {           
            base.LoadJsonConfig();
            string configPath = base.ContentRootPath + base.ConfigFileName;

            // In dev mode, load a different file.
            if (base.IsDevelopment == true)
            {
                configPath = base.ContentRootPath + "appsettings.Development.json";
            }

            using (JSONSerialiser js = new JSONSerialiser())
            {
                this.AppConfiguration = js.Deserialize<AppConfigurationModel>(new Uri(configPath));
            }
        }
    }
}
