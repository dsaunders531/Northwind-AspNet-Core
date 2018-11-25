using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using mezzanine.Attributes;
using Northwind.BLL.Validators;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CustomerCustomerDemoRowApiModel
    {
        [Required]
        [SqlInjectionCheck]
        public string CustomerId { get; set; }

        [Required]
        [ValidCustomerType()]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string CustomerTypeId { get; set; }
    }
}
