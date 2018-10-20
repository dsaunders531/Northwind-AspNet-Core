using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class RegionRowApiO
    {
        [Required]
        public int RegionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RegionDescription { get; set; }
    }
}
