using duncans.EF;
using duncans.Filters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
