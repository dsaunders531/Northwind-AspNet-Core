using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class RegionRowApiModel
    {
        [Required]
        public int RegionId { get; set; }

        [Required]
        [MaxLength(50)]
        [SqlInjectionCheck]
        public string RegionDescription { get; set; }
    }
}
