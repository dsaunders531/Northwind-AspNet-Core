using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL.Models
{
    [Table("EmployeeTerritories")]
    public partial class EmployeeTerritory
    {
        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }

        [Column("TerritoryID")]
        [MaxLength(20)]
        public string TerritoryId { get; set; }

        public Employee Employee { get; set; }
        public Territory Territory { get; set; }
    }
}
