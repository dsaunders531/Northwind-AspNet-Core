using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Northwind.BLL.Validators;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class EmployeeTerritoryRowApiO
    {
        [Required]
        [ValidEmployee()]
        public int EmployeeId { get; set; }

        [Required]
        [MaxLength(20)]
        [ValidTerritory()]
        public string TerritoryId { get; set; }
    }
}
