using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// App configuration logging configuration model.
    /// </summary>
    [NotMapped]
    public class LoggingConfigurationModel
    {
        public bool StdOutEnabled { get; set; }
        public string StdOutLevel { get; set; }
        public bool LogXMLEnabled { get; set; }
        public string LogXMLPath { get; set; }
        public string LogXMLLevel { get; set; }
        public long LogRotateMaxEntries { get; set; }
    }
}
