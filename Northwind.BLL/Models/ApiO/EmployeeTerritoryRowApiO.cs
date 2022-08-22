using Northwind.BLL.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
