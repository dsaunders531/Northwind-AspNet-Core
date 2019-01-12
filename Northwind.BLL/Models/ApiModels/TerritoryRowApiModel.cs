using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.DAL.Attributes;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class TerritoryRowApiModel : ApiModel<int>
    {
        [Required]
        [MaxLength(10)]
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
