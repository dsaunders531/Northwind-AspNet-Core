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
    public class EmployeeTerritoryRowApiModel : ApiModel<int>
    {
        [Required]
        [ValidEmployee()]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(20)]
        [ValidTerritory()]
        [SqlInjectionCheck]
        public int TerritoryId { get; set; }
    }
}
