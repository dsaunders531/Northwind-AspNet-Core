using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Northwind.DAL.Models
{
    [Table("Territories")]
    public partial class Territory
    {
        public Territory()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritory>();
        }

        [Key]
        [Column("TerritoryID")]
        [MaxLength(20)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string TerritoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TerritoryDescription { get; set; }

        [Column("RegionID")]
        public int RegionId { get; set; }

        public Region Region { get; set; }
        public ICollection<EmployeeTerritory> EmployeeTerritories { get; set; }
    }
}
