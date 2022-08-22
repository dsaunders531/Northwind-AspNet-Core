using Northwind.BLL.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CustomerCustomerDemoRowApiO
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        [ValidCustomerType()]
        [MaxLength(10)]
        public string CustomerTypeId { get; set; }
    }
}
