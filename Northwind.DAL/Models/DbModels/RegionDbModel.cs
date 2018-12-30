using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using mezzanine.Attributes;
using mezzanine.EF;

namespace Northwind.DAL.Models
{
    public partial class RegionDbModel : DbModel<int>
    {
        public RegionDbModel()
        {
            Territories = new HashSet<TerritoryDbModel>();
            Deleted = false;
            RowVersion = 1;
        }

        [Required]
        [MaxLength(50)]
        [SqlInjectionCheck]
        public string RegionDescription { get; set; }

        public ICollection<TerritoryDbModel> Territories { get; set; }
    }
}
