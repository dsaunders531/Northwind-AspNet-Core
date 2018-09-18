using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace mezzanine.Services
{
    #region "Interfaces"
    public interface IAppConfigurationService
    {
        IConfigurationRoot Configuration { get; }
        IFileProvider WebRootFileProvider { get; }
        string WebRootPath { get; }
        bool IsDevelopment { get; }
        bool IsStaging { get; }
        bool IsProduction { get; }
    }
    #endregion

    /// <summary>
    /// The configuration service from which you can expose the application configuration to 
    /// different areas of your application by injection.
    /// </summary>
    public class AppConfigurationService: IAppConfigurationService
    {
        protected string ConfigFileName { get; set; } = "appsettings.json";
        protected IHostingEnvironment Env { get; private set; } = null;
        protected string ContentRootPath { get; private set; } = string.Empty;
        protected IFileProvider ContentRootFileProvider { get; private set; } = null;

        public AppConfigurationService(IHostingEnvironment env, string JSONConfigName)
        {
            this.ConfigFileName = JSONConfigName;
            this.Load(env);
        }

        public AppConfigurationService(IHostingEnvironment env)
        {
            this.Load(env);
        }

        private void Load(IHostingEnvironment env)
        {
            // Get the JSON config file and map to object.
            // Note the configuration does not reload on change.
            // You will need to restart the app to get any config changes.
            // Its safer this way.
            this.Env = env;
            
            this.Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile(this.ConfigFileName, false, false)
                .Build();            

            this.WebRootFileProvider = env.WebRootFileProvider;

            if (env.WebRootPath.EndsWith(Path.DirectorySeparatorChar) == false)
            {
                this.WebRootPath = env.WebRootPath + Path.DirectorySeparatorChar;
            }
            else
            {
                this.WebRootPath = env.WebRootPath;
            }

            this.ContentRootFileProvider = env.ContentRootFileProvider;

            if (env.ContentRootPath.EndsWith(Path.DirectorySeparatorChar) == false)
            {
                this.ContentRootPath = env.ContentRootPath + Path.DirectorySeparatorChar;
            }
            else
            {
                this.ContentRootPath = env.ContentRootPath;
            }
 
            this.IsDevelopment = env.IsDevelopment();
            this.IsProduction = env.IsProduction();
            this.IsStaging = env.IsStaging();

            this.LoadJsonConfig();
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
