using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.BLL.Validators;
using mezzanine.Attributes;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class EmployeeTerritoryRowApiModel
    {
        [Required]
        [ValidEmployee()]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(20)]
        [ValidTerritory()]
        [SqlInjectionCheck]
        public string TerritoryId { get; set; }
    }
}
