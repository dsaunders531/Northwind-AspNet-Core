using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class ShipperRowApiModel : ApiModel<int>
    {
        [Required]
        [MaxLength(40)]
        [SqlInjectionCheck]
        public string CompanyName { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        [SqlInjectionCheck]
        public string Phone { get; set; }
    }
}
