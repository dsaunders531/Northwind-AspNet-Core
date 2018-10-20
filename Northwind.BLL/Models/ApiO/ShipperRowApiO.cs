using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class ShipperRowApiO
    {
        [Required]
        public int ShipperId { get; set; }

        [Required]
        [MaxLength(40)]
        public string CompanyName { get; set; }

        [MaxLength(24)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }
}
