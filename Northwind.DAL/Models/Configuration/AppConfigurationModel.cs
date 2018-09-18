using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// The application configuration. Used by the AppConfiguration service.
    /// This class is in the same format as the appsettings.json file.
    /// </summary>
    [NotMapped]
    public class AppConfigurationModel
    {
        public ConnectionStringsConfigurationModel ConnectionStrings { get; set; }
        public DefaultsConfigurationModel Defaults { get; set; }
        public LoggingConfigurationModel Logging { get; set; }
        public SeedDataModel SeedData { get; set; }
    }
}
