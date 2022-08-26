using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Northwind.DAL.Models;
using System;
using System.IO;
using System.Threading;
using tools.Services;
using tools.Utility;

namespace Northwind.BLL.Services
{
    /// <summary>
    /// Interface for the strongly typed version of the application configuration.
    /// </summary>
    public interface IAppConfiguration
    {
        AppConfigurationModel AppConfiguration { get; }
    }

    public sealed class AppConfigurationServiceSingleton : IAppConfiguration
    {
        private static AppConfigurationService AppConfigurationService { get; set; } = default;

        public AppConfigurationModel AppConfiguration => AppConfigurationServiceSingleton.AppConfigurationService?.AppConfiguration ?? throw new ObjectDisposedException("No configuraion! Do you need to Create()?");

        private AppConfigurationServiceSingleton(IWebHostEnvironment env)
        {
            AppConfigurationServiceSingleton.AppConfigurationService = new AppConfigurationService(env);
        }

        private static object LockObject = new object();

        public static AppConfigurationService Create(IWebHostEnvironment env)
        {
            if (Monitor.TryEnter(AppConfigurationServiceSingleton.LockObject, TimeSpan.FromSeconds(3)))
            {
                try
                {
                    if (AppConfigurationServiceSingleton.AppConfigurationService == default)
                    {
                        AppConfigurationServiceSingleton.AppConfigurationService = new AppConfigurationService(env);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    Monitor.Exit(AppConfigurationServiceSingleton.LockObject);
                }                               
            }
            else
            {
                throw new SynchronizationLockException("A lock could not be made to create the appconfigurationservice!");
            }

            return AppConfigurationServiceSingleton.AppConfigurationService;
        }
    }

    /// <summary>
    /// Use the tools configuration service to provide a strongly typed configuration in the application.
    /// </summary>
    public class AppConfigurationService : BaseAppConfigurationService, IAppConfiguration
    {
        public AppConfigurationModel AppConfiguration { get; private set; }

        public AppConfigurationService(IWebHostEnvironment env) : base(env)
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
                AppConfiguration = js.Deserialize<AppConfigurationModel>(new Uri(configPath));
            }
        }
    }

    /// <summary>
    /// The configuration service from which you can expose the application configuration to 
    /// different areas of your application by injection.
    /// </summary>
    public abstract class BaseAppConfigurationService : IAppConfigurationService
    {
        protected string ConfigFileName { get; set; } = "appsettings.json";

        protected IWebHostEnvironment Env { get; private set; } = null;
        protected string ContentRootPath { get; private set; } = string.Empty;
        protected IFileProvider ContentRootFileProvider { get; private set; } = null;

        public BaseAppConfigurationService(IWebHostEnvironment env, string JSONConfigName)
        {
            ConfigFileName = JSONConfigName;
            Load(env);
        }

        public BaseAppConfigurationService(IWebHostEnvironment env)
        {
            Load(env);
        }

        private void Load(IWebHostEnvironment env)
        {
            // Get the JSON config file and map to object.
            // Note the configuration does not reload on change.
            // You will need to restart the app to get any config changes.
            // Its safer this way.
            Env = env;

            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(ConfigFileName, false, false)
                .Build();

            WebRootFileProvider = env.WebRootFileProvider;

            if ((env.WebRootPath ?? env.ContentRootPath).EndsWith(Path.DirectorySeparatorChar) == false)
            {
                WebRootPath = (env.WebRootPath ?? env.ContentRootPath) + Path.DirectorySeparatorChar;
            }
            else
            {
                WebRootPath = (env.WebRootPath ?? env.ContentRootPath);
            }

            ContentRootFileProvider = env.ContentRootFileProvider;

            if (env.ContentRootPath.EndsWith(Path.DirectorySeparatorChar) == false)
            {
                ContentRootPath = env.ContentRootPath + Path.DirectorySeparatorChar;
            }
            else
            {
                ContentRootPath = env.ContentRootPath;
            }

            IsDevelopment = env.IsDevelopment();
            IsProduction = env.IsProduction();
            IsStaging = env.IsStaging();

            LoadJsonConfig();
        }

        /// <summary>
        /// If you want to map your appsettings.json to a strongly typed object, you need to override this 
        /// and create a property to store your strongly typed config object.
        /// </summary>
        public virtual void LoadJsonConfig() { }

        public IFileProvider WebRootFileProvider { get; private set; }

        public string WebRootPath { get; private set; }

        public IConfigurationRoot Configuration { get; private set; }

        public bool IsDevelopment { get; private set; }

        public bool IsProduction { get; private set; }

        public bool IsStaging { get; private set; }
    }
}
