using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.Models
{
    /// <summary>
    /// The error view model.
    /// </summary>
    [NotMapped]
    public class ErrorModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}