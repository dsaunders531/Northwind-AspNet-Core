using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;

namespace Northwind.DAL.Models
{
    public partial class RegionDbModel
    {
        public RegionDbModel()
        {
            Territories = new HashSet<TerritoryDbModel>();
        }

        [Key]
        [Column("RegionID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RegionId { get; set; }

        [Required]
        [MaxLength(50)]
        [SqlInjectionCheck]
        public string RegionDescription { get; set; }

        public ICollection<TerritoryDbModel> Territories { get; set; }
    }
}
