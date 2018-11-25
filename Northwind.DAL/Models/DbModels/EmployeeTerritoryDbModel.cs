using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;

namespace Northwind.DAL.Models
{
    [Table("EmployeeTerritories")]
    public partial class EmployeeTerritoryDbModel
    {
        [Key]
        [Column("EmployeeID")]
        public int EmployeeId { get; set; }

        [Column("TerritoryID")]
        [MaxLength(20)]
        [SqlInjectionCheck]
        public string TerritoryId { get; set; }

        public EmployeeDbModel Employee { get; set; }
        public TerritoryDbModel Territory { get; set; }
    }
}
