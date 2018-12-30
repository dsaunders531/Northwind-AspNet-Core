using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.DAL.Models
{
    [Table("Territories")]
    public partial class TerritoryDbModel : DbModel<int>
    {
        public TerritoryDbModel()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritoryDbModel>();
            Deleted = false;
            RowVersion = 1;
        }

        [Required]
        [MaxLength(10)]
        [SqlInjectionCheck]
        public string TerritoryId { get; set; }

        [Required]
        [MaxLength(50)]
        [SqlInjectionCheck]
        public string TerritoryDescription { get; set; }

        [Column("RegionID")]
        public int RegionId { get; set; }

        public RegionDbModel Region { get; set; }
        public ICollection<EmployeeTerritoryDbModel> EmployeeTerritories { get; set; }
    }
}
