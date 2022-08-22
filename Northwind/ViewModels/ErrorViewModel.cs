using tools.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.ViewModels
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