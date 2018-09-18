using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// App configuration connection strings model.
    /// </summary>
    [NotMapped]
    public class ConnectionStringsConfigurationModel
    {
        public string Content { get; set; }
        public string Authentication { get; set; }
    }
}
