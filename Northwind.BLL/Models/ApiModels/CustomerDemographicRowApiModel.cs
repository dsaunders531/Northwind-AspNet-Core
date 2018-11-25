using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class CustomerDemographicRowApiModel
    {
        [Required]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string CustomerTypeId { get; set; }

        [Required]
        [SqlInjectionCheck]
        public string CustomerDesc { get; set; }
    }
}
