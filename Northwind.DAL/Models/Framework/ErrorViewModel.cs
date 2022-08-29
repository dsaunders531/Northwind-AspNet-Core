using System.ComponentModel.DataAnnotations.Schema;
using tools.ViewModels;

namespace Northwind.DAL.Models.Framework
{
    /// <summary>
    /// The error view model.
    /// </summary>
    [NotMapped]
    public class ErrorViewModel : ViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
