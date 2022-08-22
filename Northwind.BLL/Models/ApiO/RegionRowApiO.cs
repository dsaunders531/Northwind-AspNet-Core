using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Northwind.BLL.Models
{
    [NotMapped]
    public class RegionRowApiO
    {
        [Required]
        public int RegionId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RegionDescription { get; set; }
    }
}
