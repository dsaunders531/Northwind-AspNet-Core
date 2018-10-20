using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.BLL.Validators;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class TerritoryRowApiO
    {
        [Required]
        [MaxLength(20)]
        public string TerritoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TerritoryDescription { get; set; }

        [Required]
        [ValidRegion()]
        public int RegionId { get; set; }
    }
}
