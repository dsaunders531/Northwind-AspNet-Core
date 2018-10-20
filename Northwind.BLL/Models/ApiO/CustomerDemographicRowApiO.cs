using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
