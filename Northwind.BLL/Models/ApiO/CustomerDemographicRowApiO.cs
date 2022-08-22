using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CustomerDemographicRowApiO
    {
        [Required]
        [MaxLength(10)]
        public string CustomerTypeId { get; set; }

        [Required]
        public string CustomerDesc { get; set; }
    }
}
