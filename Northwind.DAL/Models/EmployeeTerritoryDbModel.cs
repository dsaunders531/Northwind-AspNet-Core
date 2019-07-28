using duncans.EF;
using duncans.Filters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.DAL.Models
{
    [Table("EmployeeTerritories")]
    public partial class EmployeeTerritoryDbModel : DbModel<int>
    {
        public EmployeeTerritoryDbModel()
        {
            Deleted = false;
            RowVersion = 1;
        }

        [Column("EmployeeID")]
        public int EmployeeId { get; set; }

        [Column("TerritoryID")]
        [MaxLength(20)]
        [SqlInjectionCheck]
        public int TerritoryId { get; set; }

        public EmployeeDbModel Employee { get; set; }
        public TerritoryDbModel Territory { get; set; }
    }
}
