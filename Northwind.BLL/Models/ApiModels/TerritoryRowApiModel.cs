using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.BLL.Validators;
using mezzanine.Attributes;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class TerritoryRowApiModel
    {
        [Required]
        [MaxLength(20)]
        [SqlInjectionCheck]
        public string TerritoryId { get; set; }

        [Required]
        [MaxLength(50)]
        [SqlInjectionCheck]
        public string TerritoryDescription { get; set; }

        [Required]
        [ValidRegion()]
        public int RegionId { get; set; }
    }
}
